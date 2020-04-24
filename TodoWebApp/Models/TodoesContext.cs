using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace TodoWebApp.Models
{
    /// <summary>
    /// Todoコンテキストクラス。
    /// * データコンテキストクラス:
    ///   モデルクラスとDBを繋げる役割を果たす。DBに対するCRUD操作はすべてコンテキストクラスを使用する。
    /// </summary>
    public class TodoesContext : DbContext  // コンテキストクラスはDbContextクラスを継承する
    {
        /// <summary>
        /// DBから取得したTodoを格納するDbSet。Todoのコレクション。
        /// このプロパティを通じてデータのCRUDを行う。
        /// </summary>
        public DbSet<Todo> Todoes { get; set; }
    }
}