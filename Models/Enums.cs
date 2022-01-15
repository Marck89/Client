using System;

namespace ModelClient
{
    public enum VirtualTscCode
    {
        OK = 1,
        KO = -1,
        EMPTY = 0,
        DB_CONN= 2,
    }

    public enum VTCode
    {
        NEWUSER,
        TSCOK,
        SAMEPROFILE,
        TSCTNOK, 
        BLACKLIST,
        KO,
    }


    public enum VTContractInfoCode
    {
        OK,
        KO,
        DB_CONN,
        EMPTY,
    }


    public enum VTCfCode
    {
       SI,
       NO,
       KO,
    }


    public enum VTInsertCode
    {
        OK,
        KO,
        DB_CONN,
    }


    public enum VTDbCode
    {
        OK = 0,
        KO = 1,
        DB_CONN = -1,
        EMPTY = 2,
        NEWHOLDER = 3,
        HOLDERNOINSERT = 4,
        TSC_BLCKLIST = 5


    }


    //public enum VTDbCodeCheckCF
    //{
    //    OK = 0,
    //    KO = 1,
    //    DB_CONN = -1,
    //    EMPTY = 2,
    //    NEWHOLDER = 3,
    //    HOLDERNOINSERT = 4,
    //    TSC_BLCKLIST = 5


    //}

    public enum Lingua
    {
        IT,
        EN,
    }

    [Serializable]
    public enum CompareMode
    {
        OnlyKeys = 0,
        KeysAndValues = 1
    }
}
