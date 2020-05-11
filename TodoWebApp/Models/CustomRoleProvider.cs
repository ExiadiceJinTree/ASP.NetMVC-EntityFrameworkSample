using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace TodoWebApp.Models
{
    /// <summary>
    /// メンバーシップフレームワークによる認可の機能を司るロールロバイダー。
    /// </summary>
    public class CustomRoleProvider : RoleProvider  // RoleProvider抽象クラスを継承し、抽象メソッドを実装する必要がある。
    {
        public override string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 本アプリで実装する機能。
        /// 指定のユーザーが所属するロールを配列で返す。
        /// </summary>
        /// <param name="username">ロールの一覧を取得するユーザー。</param>
        /// <returns>指定されたユーザーに割り当てられているすべてのロールの名前を格納している文字列配列。</returns>
        public override string[] GetRolesForUser(string username)
        {
            using (var db = new TodoesContext())  // コンテキストクラスを介してDBへの操作を行う
            {
                var user = db.Users
                             .Where(u => u.UserName == username)
                             .FirstOrDefault();  // 最初に見つかった1件を返却、見つからなければdefault値(classなのでnull)を返す。
                if (user == null)
                {
                    return new string[] { };
                }

                return user.Roles.Select(r => r.RoleName).ToArray();
            }
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 指定したユーザーが指定したロールに存在するかどうかを示す値を取得します。
        /// </summary>
        /// <param name="username">検索するユーザー名。</param>
        /// <param name="roleName">検索範囲とするロール。</param>
        /// <returns>指定したユーザーが指定したロールに存在する場合は true。それ以外の場合は false。</returns>
        public override bool IsUserInRole(string username, string roleName)
        {
            string[] roleNamesForThisUser = this.GetRolesForUser(username);
            return roleNamesForThisUser.Contains(roleName);
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}