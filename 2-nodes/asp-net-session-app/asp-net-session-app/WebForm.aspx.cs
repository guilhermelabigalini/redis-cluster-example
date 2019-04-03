using Microsoft.Web.Redis;
using Recall.Common.Cache;
using Recall.Common.Cache.Providers.Memory;
using Recall.Common.Cache.Providers.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace asp_net_session_app
{
    public class MyData
    {
        public string ValueStr { get; set; }
    }

    public class CacheCollection
    {
        public static ICacheProvider Provider { get; private set; }

        static CacheCollection()
        {
            var mode = ConfigurationManager.AppSettings["cache.mode"];
            var config = ConfigurationManager.AppSettings["cache.redis.serializer.tempstoragetype"];
            if (mode == "redis")
            {
                var connection = System.Configuration.ConfigurationManager.AppSettings["cache.redis.connection"];
                var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings[connection].ConnectionString;
                var serializer = System.Configuration.ConfigurationManager.AppSettings["cache.redis.serializer"];
                ICacheSerializer serializerInstance = (serializer == "binary" ? new Recall.Common.Cache.Providers.Redis.BinarySerializer() as ICacheSerializer : new JSONSerializer());

                ITempStorage tempStorage = null;
                if (!string.IsNullOrEmpty(config))
                {
                    tempStorage = (ITempStorage)Activator.CreateInstance(Type.GetType(config));
                }
                Provider = new RedisCacheProvider(connectionString, serializerInstance, tempStorage, TimeSpan.FromSeconds(3));
            }
            else
            {
                Provider = MemoryCacheProvider.Instance;
            }
        }

    }

    public partial class WebForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            show();
        }

        protected void btncalc_Click(object sender, EventArgs e)
        {
            int v = 0;

            if (Session["value"] != null)
            {
                v = Convert.ToInt32(Session["value"]);
            }
            v++;
            Session["value"] = v;
            show();
        }
        private void show()
        {
            if (Session["value"] != null)
                lblsum.Text = Session["value"].ToString();
            else
                lblsum.Text = "-";
        }

        protected void btnGetCache_Click(object sender, EventArgs e)
        {

            var key = txtKey.Text;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var value = CacheCollection.Provider.Get<MyData>(key);
            stopwatch.Stop();

            if (value != null)
                txtValue.Text = value.ValueStr;
            else
                txtValue.Text = "NULL";

            lblLastread.Text = DateTime.Now.ToString();
            lblTime.Text = stopwatch.Elapsed.ToString();
        }

        protected void btnSetCache_Click(object sender, EventArgs e)
        {
            var key = txtKey.Text;
            var value = new MyData()
            {
                ValueStr = txtValue.Text
            };
            var d = TimeSpan.FromMinutes(int.Parse(txtDuration.Text));

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            CacheCollection.Provider.Add(key, value, d);
            stopwatch.Stop();
            lblLastread.Text = DateTime.Now.ToString();
            lblTime.Text = stopwatch.Elapsed.ToString();
        }
    }
}