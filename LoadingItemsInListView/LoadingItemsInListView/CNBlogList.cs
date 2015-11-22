using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace LoadingItemsInListView
{
    class CNBlogList : ObservableCollection<CNBlog>, ISupportIncrementalLoading
    {
        private bool _busy = false;
        private bool _has_more_items = false;
        private int _current_page = 1;
        public int TotalCount
        {
            get; set;
        }
        public bool HasMoreItems
        {
            get
            {
                if (_busy)
                    return false;
                else
                    return _has_more_items;
            }
            private set
            {
                _has_more_items = value;
            }
        }
        public CNBlogList()
        {
            HasMoreItems = true;
        }
        public void DoRefresh()
        {
            _current_page = 1;
            TotalCount = 0;
            Clear();
            HasMoreItems = true;
        }
        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            return InnerLoadMoreItemsAsync(count).AsAsyncOperation();
        }
        private async Task<LoadMoreItemsResult> InnerLoadMoreItemsAsync(uint expectedCount)
        {
            _busy = true;
            var actualCount = 0;
            List<CNBlog> list = null;
            try
            {
                list = await BlogService.GetRecentBlogsAsync(_current_page, 20);
            }
            catch (Exception)
            {
                HasMoreItems = false;
            }

            if (list != null && list.Any())
            {
                actualCount = list.Count;
                TotalCount += actualCount;
                _current_page++;
                HasMoreItems = true;
                list.ForEach((c) => { this.Add(c); });
            }
            else
            {
                HasMoreItems = false;
            }
            _busy = false;
            return new LoadMoreItemsResult
            {
                Count = (uint)actualCount
            };
        }
    }
}