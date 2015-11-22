using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace LoadingItemsInListView
{
    static class BlogService
    {
        static string _url_recent_blog = "http://wcf.open.cnblogs.com/blog/sitehome/paged/{0}/{1}"; //page_index page_size
        /// <summary>
        /// 分页获取首页博客
        /// </summary>
        /// <param name="page_index"></param>
        /// <param name="page_size"></param>
        /// <returns></returns>
        public async static Task<List<CNBlog>> GetRecentBlogsAsync(int page_index, int page_size)
        {
            try
            {
                string url = string.Format(_url_recent_blog, page_index, page_size);
                string xml = await BaseService.SendGetRequest(url);
                if (xml != null)
                {
                    List<CNBlog> list_blogs = new List<CNBlog>();
                    CNBlog cnblog;
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xml);
                    XmlNode feed = doc.ChildNodes[1];
                    foreach (XmlNode node in feed.ChildNodes)
                    {
                        if (node.Name.Equals("entry"))
                        {
                            cnblog = new CNBlog();
                            foreach (XmlNode node2 in node.ChildNodes)
                            {
                                if (node2.Name.Equals("id"))
                                {
                                    cnblog.ID = node2.InnerText;
                                }
                                if (node2.Name.Equals("title"))
                                {
                                    cnblog.Title = node2.InnerText;
                                }
                                if (node2.Name.Equals("summary"))
                                {
                                    cnblog.Summary = node2.InnerText + "...";
                                }
                                if (node2.Name.Equals("published"))
                                {
                                    DateTime t = DateTime.Parse(node2.InnerText);
                                    cnblog.PublishTime = "发表于 " + t.ToString();
                                }
                                if (node2.Name.Equals("updated"))
                                {
                                    cnblog.UpdateTime = node2.InnerText;
                                }
                                if (node2.Name.Equals("author"))
                                {
                                    cnblog.AuthorName = node2.ChildNodes[0].InnerText;
                                    cnblog.AuthorHome = node2.ChildNodes[1].InnerText;
                                    cnblog.AuthorAvator = node2.ChildNodes[2].InnerText.Equals("") ? "http://pic.cnblogs.com/avatar/simple_avatar.gif" : node2.ChildNodes[2].InnerText;
                                }
                                if (node2.Name.Equals("link"))
                                {
                                    cnblog.BlogRawUrl = node2.Attributes["href"].Value;
                                }
                                if (node2.Name.Equals("blogapp"))
                                {
                                    cnblog.BlogApp = node2.InnerText;
                                }
                                if (node2.Name.Equals("diggs"))
                                {
                                    cnblog.Diggs = node2.InnerText;
                                }
                                if (node2.Name.Equals("views"))
                                {
                                    cnblog.Views = "[" + node2.InnerText + "]";
                                }
                                if (node2.Name.Equals("comments"))
                                {
                                    cnblog.Comments = "[" + node2.InnerText + "]";
                                }
                            }
                            list_blogs.Add(cnblog);
                        }
                    }
                    return list_blogs;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
