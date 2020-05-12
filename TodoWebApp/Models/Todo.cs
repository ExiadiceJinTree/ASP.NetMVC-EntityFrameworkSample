using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace TodoWebApp.Models
{
    /// <summary>
    /// Todoモデルクラス。
    /// * モデルクラス:
    ///   データ構造を定義したクラス(POCO)。
    ///   * POCO: Plain Old CLR Object。特別なクラスやインターフェースを継承していないクラス。
    ///   EntityFrameworkのCodeFirstではPOCOを元に必要なテーブルが自動作成される。
    ///   自動作成されるテーブルの名前は、クラス名(ここではTodo)を複数形にしたテーブル名(ここではTodoes)となる。
    /// </summary>
    public class Todo
    {
        /// <summary>
        /// Entity FrameworkのCodeFirstではIdプロパティは自動的に自動作成テーブルの主キーになる。
        /// </summary>
        public int Id { get; set; }

        [DisplayName("概要")]  // モデルクラスのプロパティにDisplayName属性を指定することで、表示名を設定できる(@Html.DisplayNameForメソッドなどで表示される)。
        public string Summary { get; set; }

        [DisplayName("詳細")]
        public string Detail { get; set; }

        public DateTime Limit { get; set; }  // DisplayName属性を指定しなければ、@Html.DisplayNameForメソッドなどで表示されるのはプロパティ名となる。

        public bool Done { get; set; }

        /// <summary>
        /// Todoを保持しているユーザー。
        /// TodoモデルとUserモデルの関連を表すナビゲーションプロパティ。
        /// * ナビゲーションプロパティ: 2つのモデル(エンティティ)間の関連を表すプロパティ。
        /// * ナビゲーションプロパティは仮想プロパティである必要がある。
        ///   エンティティ上でvirtualキーワードの付いたプロパティは、それが遅延ロードされることを表す。
        ///   つまり、明示的に該当するプロパティにアクセスするまで、参照先の値(ナビゲーションプロパティの値)はDBから取得されない。
        /// </summary>
        public virtual User User { get; set; }
    }
}