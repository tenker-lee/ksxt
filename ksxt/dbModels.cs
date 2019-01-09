using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ksxt
{
    public class TbBaseModel
    {
        public int id { set; get; }
        public string create_time { set; get; }
        public string create_name { set; get; }
        public TbBaseModel()
        {
            id = -1;
            create_name = string.Empty;
            create_time = publicFun.GetDateString(DateTime.Now);
        }
    }

    public class TbChoiceModel : TbBaseModel
    {
        public int level { set; get; }
        public string title { set; get; }
        public string select_arry { set; get; }
        public string answer_arry { set; get; }

        public TbChoiceModel()
        {
            level = 0;
            title = string.Empty;
            select_arry = string.Empty;
            answer_arry = string.Empty;
        }
    }

    public class TbFillingModel : TbBaseModel
    {
        public int level { set; get; }
        public string title { set; get; }
        public string answer { set; get; }
        public TbFillingModel()
        {
            level = 0;
            title = string.Empty;
            answer = string.Empty;
        }
    }
    public class TbJudgeModel : TbBaseModel
    {
        public int level { set; get; }
        public string title { set; get; }
        public string select_arry { set; get; }
        public string answer_arry { set; get; }

        public TbJudgeModel()
        {
            level = 0;
            title = string.Empty;
            select_arry = string.Empty;
            answer_arry = string.Empty;
        }
    }

    public class TbPaperModel : TbBaseModel
    {
        public string title { set; get; }
        public string choice_id_arry { set; get; }
        public string filling_id_arry { set; get; }
        public string judge_id_arry { set; get; }
        public string qa_id_arry { set; get; }
        public string start_time { set; get; }
        public string end_time { set; get; }

        public TbPaperModel()
        {
            choice_id_arry = string.Empty;
            filling_id_arry = string.Empty;
            judge_id_arry = string.Empty;
            qa_id_arry = string.Empty;
            start_time = string.Empty;
            end_time = string.Empty;
        }
    }

    public class TbQaModel : TbBaseModel
    {
        public int level { set; get; }
        public string title { set; get; }
        public string answer { set; get; }
        public TbQaModel()
        {
            level = 0;
            title = string.Empty;
            answer = string.Empty;
        }
    }

    public class TbUserModel : TbBaseModel
    {
        public int type { set; get; }
        public string name { set; get; }
        public string password { set; get; }

        public TbUserModel()
        {
            type = 0;
            name = string.Empty;
            password = string.Empty;
        }
    }

    public class TbLoginSessionModel : TbBaseModel
    {
        public string session_key;
        public string login_time;
        public TbLoginSessionModel()
        {
            session_key = string.Empty;
            login_time = string.Empty;
        }
    }
}