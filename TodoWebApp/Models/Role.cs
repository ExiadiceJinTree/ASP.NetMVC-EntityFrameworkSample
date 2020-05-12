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
        /// * ナビゲーションプロパティ: 2つのモデル(エンティティ)間の関連を表すプロパティ。
        /// * ナビゲーションプロパティは仮想プロパティである必要がある。
        ///   エンティティ上でvirtualキーワードの付いたプロパティは、それが遅延ロードされることを表す。
        ///   つまり、明示的に該当するプロパティにアクセスするまで、参照先の値(ナビゲーションプロパティの値)はDBから取得されない。
        /// * UserとRoleはn:mの関係(UserクラスにもRolesプロパティあり)。このような場合、EntityFrameworkによりUserRolesテーブルというリレーションテーブルが作成される。
        /// </summary>
        public virtual ICollection<User> Users { get; set; }
    }
}