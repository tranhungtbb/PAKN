﻿using System;
using System.Xml;
using System.Text;

namespace KarmaloopAIMLBot.AIMLTagHandlers
{
    /// <summary>
    /// An element called bot, which may be considered a restricted version of get, is used to 
    /// tell the AIML interpreter that it should substitute the contents of a "bot predicate". The 
    /// value of a bot predicate is set at load-time, and cannot be changed at run-time. The AIML 
    /// interpreter may decide how to set the values of bot predicate at load-time. If the bot 
    /// predicate has no value defined, the AIML interpreter should substitute an empty string.
    /// 
    /// The bot element has a required name attribute that identifies the bot predicate. 
    /// 
    /// The bot element does not have any content. 
    /// </summary>
    public class pakn : KarmaloopAIMLBot.Utils.AIMLTagHandler
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="bot">The bot involved in this request</param>
        /// <param name="user">The user making the request</param>
        /// <param name="query">The query that originated this node</param>
        /// <param name="request">The request inputted into the system</param>
        /// <param name="result">The result to be passed to the user</param>
        /// <param name="templateNode">The node to be processed</param>
        public pakn(KarmaloopAIMLBot.AIMLBot bot,
                        KarmaloopAIMLBot.BotUser user,
                        KarmaloopAIMLBot.Utils.SubQuery query,
                        KarmaloopAIMLBot.Request request,
                        KarmaloopAIMLBot.Result result,
                        XmlNode templateNode)
            : base(bot, user, query, request, result, templateNode)
        {
        }

        protected override string ProcessChange()
        {
            if (this.templateNode.Name.ToLower() == "pakn")
            {
               
                    return this.templateNode.InnerText;
                
               
            }
            return string.Empty;
        }
    }
}
