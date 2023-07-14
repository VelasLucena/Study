using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudandoApi.Data.Interfaces;
using StudandoApi.Models.User;
using StudandoApi.Properties.Enuns;
using SudyApi.Properties.Enuns;

namespace StudandoApi.Controllers
{
    public class UserController : Controller
    {
        private readonly ISudyService _sudyService;

        public UserController(ISudyService schoolService)
        {
            _sudyService = schoolService;
        }

        [HttpGet]
        [ActionName(nameof(GetUsers))]
        [Authorize]
        public async Task<IActionResult> GetUsers(int limit = 100, Ordering ordering = Ordering.Desc, string? attributeName = nameof(UserModel.UserId))
        {
            try
            {
                List<UserModel> users = await _sudyService.UserRepository.GetUsersNoTracking(limit, ordering, attributeName);

                if (users.Count == 0)
                    return NotFound();

                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpGet]
        [ActionName(nameof(GetUserById))]
        [Authorize]
        public async Task<IActionResult> GetUserById(int userId)
        {
            try
            {
                UserModel user = await _sudyService.UserRepository.GetUserByIdNoTracking(userId);

                if (user == null)
                    return NotFound();

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpGet]
        [ActionName(nameof(GetUserByName))]
        [Authorize]
        public async Task<IActionResult> GetUserByName(string name)
        {
            try
            {
                List<UserModel> users = await _sudyService.UserRepository.GetUserByNameNoTracking(name);

                if (users == null || users.Count == 0)
                    return NotFound();

                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPost]
        [ActionName(nameof(CreateUser))]
        public async Task<IActionResult> CreateUser(RegisterUserViewModel user)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();

                ClassroomModel classroom = new ClassroomModel();

                if (user.ClassId != null)
                {
                    if (user.AcessType == AcessType.Student)
                        classroom = await _schoolService.ClassroomRepository.GetClassroomById(Convert.ToInt32(user.ClassId));

                    if (classroom == null)
                        return NotFound(new { Message = MessageClient.MC0039 });
                }

                UserModel newUser = new UserModel(user);

                await _schoolService.Create(newUser);

                switch (newUser.AcessType)
                {
                    case AcessType.Professor:
                        await _schoolService.Create(new TeacherModel(newUser, user));
                        break;

                    case AcessType.Student:
                        if (user.ClassId != null)
                        {
                            await _schoolService.Create(new StudentModel(classroom, newUser));
                        }
                        else
                            await _schoolService.Create(new StudentModel(newUser));
                        break;
                }

                return Ok(newUser);
            }
            catch (Exception ex)
            {
                LoggerSystem.Error(ex);
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPost]
        [ActionName(nameof(EditUser))]
        [Authorize]
        public async Task<IActionResult> EditUser(EditUserViewModel userEdit)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();

                if (!await ExistUser(userEdit.UserId))
                    return NotFound();

                UserModel userOld = await _schoolService.UsersRepository.GetUserByIdNoTracking(userEdit.UserId);

                if (userOld.AcessType != userEdit.AcessType)
                {
                    switch (userOld.AcessType)
                    {
                        case AcessType.Student:
                            StudentModel student = await _schoolService.StudentRepository.GetStudentByUserIdNoForeign(userEdit.UserId);
                            if (student != null)
                                await _schoolService.Delete(student);
                            break;

                        case AcessType.Professor:
                            TeacherModel teacher = await _schoolService.TeacherRepository.GetTeacherByUserIdNoForeign(userEdit.UserId);
                            if (teacher != null)
                                await _schoolService.Delete(teacher);
                            break;
                    }
                }


                UserModel userNew = new UserModel(userEdit);

                await _schoolService.Update(userNew);

                switch (userEdit.AcessType)
                {
                    case AcessType.Professor:
                        TeacherModel teacher = await _schoolService.TeacherRepository.GetTeacherByUserIdNoForeign(userEdit.UserId);
                        if (teacher != null)
                        {
                            teacher.Update(userEdit, userNew);
                            await _schoolService.Update(teacher);
                        }
                        else
                            await _schoolService.Create(new TeacherModel(userNew));
                        break;

                    case AcessType.Student:
                        StudentModel student = await _schoolService.StudentRepository.GetStudentByUserIdNoForeign(userEdit.UserId);
                        if (student != null)
                        {
                            if (userEdit.classroomId != null && userEdit.AcessType == Acess.AcessType.Student)
                                student.Class = await _schoolService.ClassroomRepository.GetClassroomById(Convert.ToInt32(userEdit.classroomId));

                            student.Update(student.Class, userNew);
                            await _schoolService.Update(student);
                        }
                        else
                        {
                            ClassroomModel classroom = new ClassroomModel();
                            if (userEdit.classroomId != null && userEdit.AcessType == Acess.AcessType.Student)
                                classroom = await _schoolService.ClassroomRepository.GetClassroomById(Convert.ToInt32(userEdit.classroomId));
                            await _schoolService.Create(new StudentModel(classroom, userNew));
                        }
                        break;
                }

                return Ok(userNew);
            }
            catch (Exception ex)
            {
                LoggerSystem.Error(ex);
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpDelete]
        [ActionName(nameof(DeleteUser))]
        [Authorize]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            try
            {
                UserModel user = await _schoolService.UsersRepository.GetUserById(userId);

                if (user == null)
                    return NotFound();

                switch (user.AcessType)
                {
                    case AcessType.Professor:
                        TeacherModel teacher = await _schoolService.TeacherRepository.GetTeacherByUserId(userId);
                        if (teacher != null)
                            await _schoolService.Delete(teacher);
                        else
                            return NotFound();
                        break;

                    case AcessType.Student:
                        StudentModel student = await _schoolService.StudentRepository.GetStudentByUserId(userId);
                        if (student != null)
                            await _schoolService.Delete(student);
                        else
                            return NotFound();
                        break;
                }

                await _schoolService.Delete(user.UserInformation);
                await _schoolService.Delete(user);

                return Ok();
            }
            catch (Exception ex)
            {
                LoggerSystem.Error(ex);
                return BadRequest(new { Error = ex.Message });
            }
        }
    }
}
