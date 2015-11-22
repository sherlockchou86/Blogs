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

namespace LoadingItemsInListView
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        CNBlogList _binding_list_blogs;

        int _unbinding_current_page = 1;
        bool _unbinding_busy = false;
        public MainPage()
        {
            this.InitializeComponent();

            //BindingListView绑定到数据源  数据源自动加载数据
            BindingListView.ItemsSource = _binding_list_blogs = new CNBlogList();

            //手动向UnBindingListView中添加一个与要显示的数据不同的项
            ListViewItem different_item = new ListViewItem();
            different_item.Content = "我是单独不同的一项";
            different_item.HorizontalContentAlignment = HorizontalAlignment.Center;
            UnBindingListView.Items.Add(different_item);

            //手动向UnBindingListView中添加一个按钮项  点击手动加载数据
            ListViewItem loading_more = new ListViewItem();
            loading_more.Content = "点击加载更多...";
            loading_more.HorizontalContentAlignment = HorizontalAlignment.Center;
            loading_more.Tapped += loading_more_Tapped;
            UnBindingListView.Items.Add(loading_more);
        }

        /// <summary>
        /// 点击加载更多
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void loading_more_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (_unbinding_busy)
                return;
            _unbinding_busy = true;
            (sender as ListViewItem).Content = "正在加载...";

            List<CNBlog> loading_item = await BlogService.GetRecentBlogsAsync(_unbinding_current_page, 20);
            if (loading_item != null)
            {
                loading_item.ForEach((blog) =>
                {
                    CNBlogItem item = new CNBlogItem(blog);
                    UnBindingListView.Items.Insert(UnBindingListView.Items.Count - 1, item);
                });
                _unbinding_current_page++;
                UnBindingListView.ScrollIntoView(sender);
            }
            (sender as ListViewItem).Content = "点击加载更多...";
            _unbinding_busy = false;
        }
    }
}
