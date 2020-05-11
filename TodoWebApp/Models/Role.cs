using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TodoWebApp.Models
{
    public class Role
    {
        /// <summary>
        /// Entity FrameworkのCodeFirstではIdプロパティは自動的に自動作成テーブルの主キーになる。
        /// </summary>
        public int Id { get; set; }
        
        public string RoleName { get; set; }

        /// <summary>
        /// このロールに所属するユーザーのコレクション(ロールに複数のユーザーが所属できるように)。
        /// RoleモデルとUserモデルの関連を表すナビゲーションプロパティ。
        /// * ナビゲーションプロパティの説明と仮想プロパティ(遅延ロードプロパティ)の説明はUserクラスのRolesプロパティの説明にも書いているのでそちらを参照のこと。
        /// * UserとRoleはn:mの関係(UserクラスにもRolesプロパティあり)。このような場合、EntityFrameworkによりUserRolesテーブルというリレーションテーブルが作成される。
        /// </summary>
        public virtual ICollection<User> Users { get; set; }
    }
}