namespace SudyApi.Properties.Enuns
{
    public enum Database
    {
        SudyData = 1
    }

    public enum Ordering
    {
        Asc = 1,
        Desc = 2,
    }

    public enum ConfigKeys
    {
        RedisCache = 1,
        JwtKey = 2,
        ElasticSearchUrl = 3
    }

    public enum ClaimTypeToken
    {
        nameid = 1
    }
}
