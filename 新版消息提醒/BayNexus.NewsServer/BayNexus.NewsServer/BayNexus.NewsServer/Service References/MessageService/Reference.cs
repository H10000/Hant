﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace BayNexus.NewsServer.MessageService {
    using System.Data;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="MessageService.MessageServiceSoap")]
    public interface MessageServiceSoap {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetCurrentMessage", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string GetCurrentMessage(string receiveID, string sendID, string messageType, int sendStatus);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetCurrentMessage", ReplyAction="*")]
        System.Threading.Tasks.Task<string> GetCurrentMessageAsync(string receiveID, string sendID, string messageType, int sendStatus);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/SaveMessage", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        void SaveMessage(string messageContent, string messageType, string sendID, string sendName, string receiveID, string receiveName, string sendTime, string sendStatus, string createTime);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/SaveMessage", ReplyAction="*")]
        System.Threading.Tasks.Task SaveMessageAsync(string messageContent, string messageType, string sendID, string sendName, string receiveID, string receiveName, string sendTime, string sendStatus, string createTime);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetMessage", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataSet GetMessage(string ReceiveIDs, string MessageType, int SendStatus);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetMessage", ReplyAction="*")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetMessageAsync(string ReceiveIDs, string MessageType, int SendStatus);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/UpdateMessageStatus", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        void UpdateMessageStatus(int sendStatus, string messageIDs);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/UpdateMessageStatus", ReplyAction="*")]
        System.Threading.Tasks.Task UpdateMessageStatusAsync(int sendStatus, string messageIDs);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface MessageServiceSoapChannel : BayNexus.NewsServer.MessageService.MessageServiceSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class MessageServiceSoapClient : System.ServiceModel.ClientBase<BayNexus.NewsServer.MessageService.MessageServiceSoap>, BayNexus.NewsServer.MessageService.MessageServiceSoap {
        
        public MessageServiceSoapClient() {
        }
        
        public MessageServiceSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public MessageServiceSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public MessageServiceSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public MessageServiceSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string GetCurrentMessage(string receiveID, string sendID, string messageType, int sendStatus) {
            return base.Channel.GetCurrentMessage(receiveID, sendID, messageType, sendStatus);
        }
        
        public System.Threading.Tasks.Task<string> GetCurrentMessageAsync(string receiveID, string sendID, string messageType, int sendStatus) {
            return base.Channel.GetCurrentMessageAsync(receiveID, sendID, messageType, sendStatus);
        }
        
        public void SaveMessage(string messageContent, string messageType, string sendID, string sendName, string receiveID, string receiveName, string sendTime, string sendStatus, string createTime) {
            base.Channel.SaveMessage(messageContent, messageType, sendID, sendName, receiveID, receiveName, sendTime, sendStatus, createTime);
        }
        
        public System.Threading.Tasks.Task SaveMessageAsync(string messageContent, string messageType, string sendID, string sendName, string receiveID, string receiveName, string sendTime, string sendStatus, string createTime) {
            return base.Channel.SaveMessageAsync(messageContent, messageType, sendID, sendName, receiveID, receiveName, sendTime, sendStatus, createTime);
        }
        
        public System.Data.DataSet GetMessage(string ReceiveIDs, string MessageType, int SendStatus) {
            return base.Channel.GetMessage(ReceiveIDs, MessageType, SendStatus);
        }
        
        public System.Threading.Tasks.Task<System.Data.DataSet> GetMessageAsync(string ReceiveIDs, string MessageType, int SendStatus) {
            return base.Channel.GetMessageAsync(ReceiveIDs, MessageType, SendStatus);
        }
        
        public void UpdateMessageStatus(int sendStatus, string messageIDs) {
            base.Channel.UpdateMessageStatus(sendStatus, messageIDs);
        }
        
        public System.Threading.Tasks.Task UpdateMessageStatusAsync(int sendStatus, string messageIDs) {
            return base.Channel.UpdateMessageStatusAsync(sendStatus, messageIDs);
        }
    }
}
