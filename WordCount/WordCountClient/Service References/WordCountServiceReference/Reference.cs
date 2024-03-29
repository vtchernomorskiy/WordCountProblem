﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18449
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WordCountClient.WordCountServiceReference {
    using System.Runtime.Serialization;
    using System;
    using WordCountLibrary.Interfaces;
  
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IWordCountChannel : IWordCount, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class WordCountClient : System.ServiceModel.ClientBase<IWordCount>, IWordCount {
        
        public WordCountClient() {
        }
        
        public WordCountClient(string endpointConfigurationName) 
                : base(endpointConfigurationName) {
        }
        
        public WordCountClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WordCountClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WordCountClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public UserResponse CountWordsInStatement(string statement, bool removePunctuation) {
            return base.Channel.CountWordsInStatement(statement, removePunctuation);
        }
    }
}
