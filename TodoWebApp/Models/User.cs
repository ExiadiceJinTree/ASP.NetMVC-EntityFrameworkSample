using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TodoWebApp.Models
{
    public class User
    {
        /// <summary>
        /// Entity FrameworkのCodeFirstではIdプロパティは自動的に自動作成テーブルの主キーになる。
        /// </summary>
        public int Id { get; set; }

        [Required]
        [Index(IsUnique = true)]  // Index属性を付与すると、このプロパティがマップされているDB列にインデックスがあることを示せ、IsUnique=trueに設定することでユニーク制約を設定できる。
        [StringLength(256)]  // フィールドの最大文字長を設定。string型にUnique設定すると、Databaseマイグレーション時にエラーとなる(SQLServerのindex keyの最大長が900Byteのため)。最大長を設定することで解決。
        [DisplayName("ユーザー名")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(6)]
        [DisplayName("パスワード")]
        public string Password { get; set; }

        /// <summary>
        /// ユーザーが所属するロールのコレクション(ユーザーが複数のロールに所属できるように)。
        /// UserモデルとRoleモデルの関連を表すナビゲーションプロパティ。
        /// * ナビゲーションプロパティ: 2つのモデル(エンティティ)間の関連を表すプロパティ。
        /// * ナビゲーションプロパティは仮想プロパティである必要がある。
        ///   エンティティ上でvirtualキーワードの付いたプロパティは、それが遅延ロードされることを表す。
        ///   つまり、明示的に該当するプロパティにアクセスするまで、参照先の値(ここでは関連付いたRoleテーブルの値)はDBから取得されない。
        /// * UserとRoleはn:mの関係(RoleクラスにもUsersプロパティあり)。このような場合、EntityFrameworkによりUserRolesテーブルというリレーションテーブルが作成される。
        /// </summary>
        public virtual ICollection<Role> Roles { get; set; }

        /// <summary>
        /// ユーザー作成/編集画面で選択されたロールを保持するためのロールリスト。
        /// * プロパティ名: UsersControllerで指定したViewBagのキー名と同じ。
        /// </summary>
        //[Required]
        [NotMapped]  // DBに保持する必要がないため、DBマッピングから除外する(マイグレーション処理から無視される)。
        [DisplayName("ロール")]
        public List<int> RoleIds { get; set; }
    }
}