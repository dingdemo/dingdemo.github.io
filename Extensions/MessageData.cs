using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ding
{
    /// <summary>
    /// Json消息类
    /// </summary>
    public class MessageData
    {
        public MessageData(StatusType status, string message)
        {
            this.Status = status;
            this.Message = message;
        }

        public MessageData(bool success, string message)
        {
            if (success)
                this.Status = StatusType.Success;
            else
                this.Status = StatusType.Error;
            this.Message = message;
        }

        public MessageData(bool success, string message, int data)
        {
            if (success)
                this.Status = StatusType.Success;
            else
                this.Status = StatusType.Error;
            this.Message = message;
            this.Data = data;
        }
        /// <summary>
        /// 状态 成功1,失败-1,警告0
        /// </summary>
        public StatusType Status { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public int Data { get; set; }
    }

    public enum StatusType
    {
        Warn = 0,
        Success = 1,
        Error = 2,
        Show = 3
    }
}
