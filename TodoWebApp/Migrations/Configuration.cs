/// <summary>
/// EntityFrameworkの自動Migration機能を有効にするための設定。
/// NuGetパッケージマネージャーコンソールで、以下のコマンドを実行することで、Migrationsフォルダと本クラスファイルを自動生成。
///   > Enable-Migrations -EnableAutomaticMigrations
/// * Migration機能: Modelの変更に合わせてDBにその内容が反映(作成・変更)される仕組み。
/// </summary>
namespace TodoWebApp.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using TodoWebApp.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<TodoWebApp.Models.TodoesContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;  // データ損失となるモデルの変更(列の削除等)についても自動マイグレーション(DBへの自動反映)を許可する。 * 自動生成後の手動追加設定。
            ContextKey = "TodoWebApp.Models.TodoesContext";
        }

        protected override void Seed(TodoWebApp.Models.TodoesContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.

            //****************************************************************************************************
            // 以下、自動生成後の手動追加コード:
            // ・UserとRoleの初期データを登録する処理を実装。
            //   - Userの初期データ: 管理者, 一般ユーザー
            //   - Roleの初期データ: Administrators(管理者), Users(一般ユーザー)
            //****************************************************************************************************
            //====================================================================================================
            // User用意
            //====================================================================================================
            User admin = new User()  // 管理者
            {
                Id = 1,
                UserName = "admin",
                Password = "password",
                Roles = new List<Role>(),
                //RoleIds = new List<int>(),
            };

            User generalUser01 = new User()  // 一般ユーザー
            {
                Id = 2,
                UserName = "GeneralUser01",
                Password = "password",
                Roles = new List<Role>(),
                //RoleIds = new List<int>(),
            };

            //====================================================================================================
            // Role用意
            //====================================================================================================
            Role administrators = new Role()  // Administrators(管理者)
            {
                Id = 1,
                RoleName = "Administrators",
                Users = new List<User>(),
            };

            Role users = new Role()  // Users(一般ユーザー)
            {
                Id = 2,
                RoleName = "Users",
                Users = new List<User>(),
            };

            //====================================================================================================
            // User-Role設定
            //====================================================================================================
            admin.Roles.Add(administrators);
            //admin.RoleIds.Add(administrators.Id);
            administrators.Users.Add(admin);

            generalUser01.Roles.Add(users);
            //generalUser01.RoleIds.Add(users.Id);
            users.Users.Add(generalUser01);

            //====================================================================================================
            // UserとRoleの情報をDBに反映
            // * 本Seedメソッドの引数のcontextパラメータを利用してDBにアクセス。
            // *  DbSet<T>.AddOrUpdate(identifierExpression, entities)メソッド: DBに既存でなければ追加、既存なら更新。第1引数identifierExpressionに指定した値と同等のキーが存在すればAdd、存在しなければUpdate。
            //====================================================================================================
            context.Users.AddOrUpdate(user => user.Id, new User[] { admin, generalUser01 });
            context.Roles.AddOrUpdate(role => role.Id, new Role[] { administrators, users });
        }
    }
}
