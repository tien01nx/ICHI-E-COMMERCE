using ICHI_CORE.Domain;

namespace ICHI_CORE.Model
{
    public class ResGennerateModel
    {
        /// <summary>
        /// Resource Name
        /// </summary>
        public string TypeResource { get; set; }

        /// <summary>
        /// ADD, UPDATE, DELETE
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// Json data for resource
        /// </summary>
        public string JsonData { get; set; }

        /// <summary>
        /// Message Error
        /// </summary>
        public string Message { get; set; }

        public ResGennerateModel(string _TypeResource, string _Action, string _JsonData, string _Message)
        {
            TypeResource = _TypeResource;
            Action = _Action;
            JsonData = _JsonData;
            Message = _Message;
        }
    }

    public class ReqGennerateModel
    {

    }
}
