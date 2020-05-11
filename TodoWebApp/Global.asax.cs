using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using TodoWebApp.Migrations;
using TodoWebApp.Models;

namespace TodoWebApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// 本プロジェクトアプリケーションが起動したときに最初に呼ばれるメソッド。
        /// </summary>
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            //****************************************************************************************************
            // 以下、自動生成後の手動追加コード:
            //****************************************************************************************************
            // EntityFrameworkの初期化処理 - Migrations.Configurationクラスの実行
            // * 以下のようにすることで、DBの自動マイグレーションを行え、Migrations.ConfigurationクラスのSeedメソッドを呼び初期データ自動投入も行える。
            // * Database.SetInitializerメソッド: DBの初期化方法を設定する。DBの自動生成方法をカスタマイズできる。
            //   - SetInitializerメソッドの引数: 初期化方法を示すIDatabaseInitializerインターフェイスを実装したクラスを指定。あらかじめいくつかのクラスが用意されている。
            //   - SetInitializerメソッドにMigrateDatabaseToLatestVersionインスタンスを渡すことでDBの自動マイグレーションを行うことができる。
            //   - 初期データを投入するには、MigrateDatabaseToLatestVersionクラスの型引数であるMigrations.ConfigurationクラスのSeedメソッドに、初期データ投入用コードを記述する。
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<TodoesContext, Configuration>());
        }
    }
}
