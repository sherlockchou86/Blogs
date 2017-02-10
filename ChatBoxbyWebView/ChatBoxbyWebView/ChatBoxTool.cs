using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace ChatBoxbyWebView
{
    /// <summary>
    /// 聊天框工具
    /// </summary>
    class ChatBoxTool
    {
        static string _bastChatHtml = @"<html><head>
        <script type=""text/javascript"">window.location.hash = ""#ok"";</script>
        <style type=""text/css"">
        body{
        font-family:微软雅黑;
        font-size:14px;
        }
        /*滚动条宽度*/  
        ::-webkit-scrollbar {  
        width: 8px;  
        }  
   
        /* 轨道样式 */  
        ::-webkit-scrollbar-track {  
        }  
   
        /* Handle样式 */  
        ::-webkit-scrollbar-thumb {  
        border-radius: 10px;  
        background: rgba(0,0,0,0.2);   
        }  
  
        /*当前窗口未激活的情况下*/  
        ::-webkit-scrollbar-thumb:window-inactive {  
        background: rgba(0,0,0,0.1);   
        }  
  
        /*hover到滚动条上*/  
        ::-webkit-scrollbar-thumb:vertical:hover{  
        background-color: rgba(0,0,0,0.3);  
        }  
        /*滚动条按下*/  
        ::-webkit-scrollbar-thumb:vertical:active{  
        background-color: rgba(0,0,0,0.7);  
        }  
        textarea{width: 500px;height: 300px;border: none;padding: 5px;}  

	    .chat_content_group.self {
        text-align: right;
        }
        .chat_content_group {
        padding: 5px;
        }
        .chat_content_group.self>.chat_content {
        text-align: left;
        }
        .chat_content_group.self>.chat_content {
        background: #7ccb6b;
        color:#fff;
        /*background: -webkit-gradient(linear,left top,left bottom,from(white,#e1e1e1));
        background: -webkit-linear-gradient(white,#e1e1e1);
        background: -moz-linear-gradient(white,#e1e1e1);
        background: -ms-linear-gradient(white,#e1e1e1);
        background: -o-linear-gradient(white,#e1e1e1);
        background: linear-gradient(#fff,#e1e1e1);*/
        }
        .chat_content {
        display: inline-block;
        min-height: 16px;
        max-width: 50%;
        color:#292929;
        background: #c3f1fd;
        font-family:微软雅黑;
        font-size:14px;
        /*background: -webkit-gradient(linear,left top,left bottom,from(#cf9),to(#9c3));
        background: -webkit-linear-gradient(#cf9,#9c3);
        background: -moz-linear-gradient(#cf9,#9c3);
        background: -ms-linear-gradient(#cf9,#9c3);
        background: -o-linear-gradient(#cf9,#9c3);
        background: linear-gradient(#cf9,#9c3);*/
        -webkit-border-radius: 5px;
        -moz-border-radius: 5px;
        border-radius: 5px;
        padding: 10px 15px;
        word-break: break-all;
        /*box-shadow: 1px 1px 5px #000;*/
        line-height: 1.4;
        }

        .chat_content_group.self>.chat_nick {
        text-align: right;
        }
        .chat_nick {
        font-size: 14px;
        margin: 0 0 10px;
        color:#8b8b8b;
        }

        .chat_content_group.self>.chat_content_avatar {
        float: right;
        margin: 0 0 0 10px;
        }

        .chat_content_group.buddy {
        text-align: left;
        }
        .chat_content_group {
        padding: 10px;
        }
        .chat_content_avatar {
        float: left;
        width: 40px;
        height: 40px;
        margin-right: 10px;
        }
        .imgtest{margin:10px 5px;
        overflow:hidden;}
        .list_ul figcaption p{
        font-size:11px;
        color:#aaa;
        }
        .imgtest figure div{
        display:inline-block;
        margin:5px auto;
        width:100px;
        height:100px;
        border-radius:100px;
        border:2px solid #fff;
        overflow:hidden;
        -webkit-box-shadow:0 0 3px #ccc;
        box-shadow:0 0 3px #ccc;
        }
        .imgtest img{width:100%;
        min-height:100%; text-align:center;}
	    </style>
        </head><body></body></html>";

        //接收消息html
        //点击昵称、头像时  js与C#代码进行通信  window.external.notify(...)
        static string _receiveHtml = @" 
            <div class=""chat_content_group buddy"">   
            <img class=""chat_content_avatar"" onclick=""window.external.notify('{3}')"" style=""cursor:pointer"" src=""{0}"" width=""40px"" height=""40px""/> 
            <p class=""chat_nick"" style=""cursor:pointer;font-family:微软雅黑"" onclick=""window.external.notify('{4}')"">{1}</p>   
            <p class=""chat_content"">{2}</p>
            </div><a id=""ok""></a>";
        //发送消息html
        static string _sendHtml = @" 
            <div class=""chat_content_group self"">   
            <img class=""chat_content_avatar"" src=""{0}"" width=""40px"" height=""40px""/>  
            <p class=""chat_nick"" style=""font-family:微软雅黑"">{1}</p>   
            <p class=""chat_content"">{2}</p>
            </div><a id=""ok""></a>";


        /// <summary>
        /// 当前聊天框
        /// </summary>
        private WebView _chat_box;
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="wv"></param>
        public ChatBoxTool(WebView wv)
        {
            _chat_box = wv;
            _chat_box.NavigateToString(_bastChatHtml);          
        }
        /// <summary>
        /// 聊天框接收消息
        /// </summary>
        /// <param name="avatar"></param>
        /// <param name="nickname"></param>
        /// <param name="content"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public async void Receive(string avatar, string nickname, string content, string time)
        {
            //将消息转换成html
            string html_2_insert = String.Format(_receiveHtml, avatar, nickname + " " + time, content, nickname, nickname);
            
            //C# 与 js通信
            string js = "";  
            js += "var div = document.createElement('div');";  //创建div
            js += "div.innerHTML='" + html_2_insert.Replace("\"","\\\"").Replace("'", "\\'").Replace("\r\n","") + "';"; //插入html
            js += "document.body.appendChild(div);";  //将div添加到body中
            js += "location.href='#ok';";  //webview定位到最新一条消息
            js += "document.getElementById('ok').remove();"; //将锚点移除

            await _chat_box.InvokeScriptAsync("eval", new string[] { js });  //调用js
        }
        /// <summary>
        /// 聊天框发送消息
        /// </summary>
        /// <param name="avatar"></param>
        /// <param name="nickname"></param>
        /// <param name="content"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public async void Send(string avatar, string nickname, string content, string time)
        {
            //将消息转换成html
            string html_2_insert = String.Format(_sendHtml, avatar, time + " " + nickname, content);

            //C# 与 js通信
            string js = "";
            js += "var div = document.createElement('div');";  //创建div
            js += "div.innerHTML=\"" + html_2_insert.Replace("\"", "\\\"").Replace("\r\n", "") + "\";"; //插入html
            js += "document.body.appendChild(div);";  //将div添加到body中
            js += "location.href='#ok';";  //webview定位到最新一条消息
            js += "document.getElementById('ok').remove();"; //将锚点移除

            await _chat_box.InvokeScriptAsync("eval", new string[] { js });  //调用js
        }
    }
}
