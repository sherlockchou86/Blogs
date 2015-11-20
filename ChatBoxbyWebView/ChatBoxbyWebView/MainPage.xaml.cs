using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace ChatBoxbyWebView
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        ChatBoxTool _chat_box_tool;
        public MainPage()
        {
            this.InitializeComponent();
            _chat_box_tool = new ChatBoxTool(ChatBox);
            ChatBox.ScriptNotify += ChatBox_ScriptNotify; //js 与 C#通信
        }
        /// <summary>
        /// 模拟接收文本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //模拟产生的消息数据
            string avatar = "http://pic.cnblogs.com/avatar/104032/20150821151916.png";
            string nick_name = "周见智";
            string content = "模拟接收的加粗文本<br><br><b>你好！完美世界！</b>";
            string time = DateTime.Now.ToString();

            _chat_box_tool.Receive(avatar, nick_name, content, time);  //插入聊天框
        }
        /// <summary>
        /// 模拟接收图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //模拟产生的消息数据
            string avatar = "http://pic.cnblogs.com/avatar/104032/20150821151916.png";
            string nick_name = "周见智";
            string content = "模拟接收的图片<br><br><img width='300'src='http://images2015.cnblogs.com/news/66372/201511/66372-20151120125441936-1204663180.jpg'/>";
            string time = DateTime.Now.ToString();

            _chat_box_tool.Receive(avatar, nick_name, content, time);  //插入聊天框
        }
        /// <summary>
        /// 模拟接收视频
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //模拟产生的消息数据
            string avatar = "http://pic.cnblogs.com/avatar/104032/20150821151916.png";
            string nick_name = "周见智";
            string content = "模拟接收的视频<br><iframe src=\"http://player.youku.com/embed/XMTM5MTAyMjQ3Mg==\" frameborder=0></iframe>";
            string time = DateTime.Now.ToString();

            _chat_box_tool.Receive(avatar, nick_name, content, time);  //插入聊天框
        }
        /// <summary>
        /// 模拟发送文本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (!Text2Send.Text.Equals(""))
            {
                string avatar = "http://pic.cnblogs.com/avatar/624159/20150505133758.png";
                string nick_name = "周加祖";
                string content = Text2Send.Text;
                string time = DateTime.Now.ToString();

                _chat_box_tool.Send(avatar, nick_name, content, time);  //插入聊天框
            }
        }

        /// <summary>
        /// 模拟发送超链接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            string avatar = "http://pic.cnblogs.com/avatar/624159/20150505133758.png";
            string nick_name = "周加祖";
            string content = "<b>发送一条链接,关注我博客</b><br><a href='http://www.cnblogs.com/xiaozhi_5638/' target='_blank'>www.cnblogs.com/xiaozhi_5638/</a>";
            string time = DateTime.Now.ToString();
            _chat_box_tool.Send(avatar, nick_name, content, time);  //插入聊天框
        }


        /// <summary>
        /// 鼠标点击webview中的昵称   js向C#传递’点击的昵称‘
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChatBox_ScriptNotify(object sender, NotifyEventArgs e)
        {
            //将昵称添加到输入框
            Text2Send.Text += "@" + e.Value + " ";   //点击昵称  @该用户
        }
    }
}
