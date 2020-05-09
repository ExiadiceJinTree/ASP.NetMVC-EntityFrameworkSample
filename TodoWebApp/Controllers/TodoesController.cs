using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TodoWebApp.Models;

namespace TodoWebApp.Controllers
{
    /// <summary>
    /// Todoのコントローラー。
    /// コントローラー生成テンプレート"Entity Frameworkを使用した、ビューがあるMVC5コントローラー"で自動生成。
    /// コントローラー自動生成時のオプション:
    ///   ・モデルクラス=Todo  ・データコンテキストクラス=TodoesContext  ・非同期コントローラーアクションの使用=false
    ///   ・ビュー:
    ///     ・ビューの生成=true  ・スクリプトライブラリの参照=true  ・レイアウトページの使用=true(~/Views/_LayoutPage1.cshtml)
    /// </summary>
    [Authorize]  // 本コントローラーには認証された状態でないとアクセスできないようにする。
    public class TodoesController : Controller  // Controllerクラスを継承した独自クラスを継承してもよい
    {
        private TodoesContext db = new TodoesContext();  // コンテキストクラスを介してDBへの操作を行う

        // GET: Todoes
        public ActionResult Index()
        {
            return View(db.Todoes.ToList());  // Viewページ Index.cshtmlにTodoes.ToList()結果を渡してViewResultを返す
        }

        // GET: Todoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Todo todo = db.Todoes.Find(id);
            if (todo == null)
            {
                return HttpNotFound();
            }
            return View(todo);
        }

        // GET: Todoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Todoes/Create
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        // * モデルバインド機能とは:
        //     クライアントからの入力値を自動的にPOCOに割り当てる機能。Postされてきたデータのキー名を見て、引数のPOCOに自動的に割り当ててくれる。
        //     アクションメソッドはバインドを使わずに書くこともできる(以下例):
        //       public ActionResult Create(Todo todo) { ...
        //     このように書いてもモデルバインド機能は問題なく動作するが、これでは過多ポスティング攻撃を防げない。
        // * 過多ポスティング攻撃と対策:
        //     アクションメソッドでモデルバインド先とするプロパティを明示していない場合、過多ポスティング攻撃を防げない。
        //     例えばCreateビューからDoneプロパティを入力できないよう非表示にしても、クライアント側でHTMLを作成/編集してDoneプロパティに値を入れてPOSTできてしまう。
        //     これが過多ポスティング攻撃。これを防ぐには、アクションメソッドでモデルバインド先とするプロパティを明示する必要がある。
        //     そうすれば、バインド先プロパティに指定していないプロパティが入力された状態でPOSTされても、バインド先プロパティのみが利用される(エラーにはならない)。
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Create(Todo todo)  // モデルバインド先とするプロパティを明示していない場合
        //public ActionResult Create([Bind(Include = "Id,Summary,Detail,Limit,Done")] Todo todo)  // モデルバインド先とするプロパティを明示している場合 (Doneも指定している)
        public ActionResult Create([Bind(Include = "Id,Summary,Detail,Limit")]　Todo todo)  // モデルバインド先とするプロパティを明示している場合 (Doneを指定しない)
        {
            if (ModelState.IsValid)
            {
                db.Todoes.Add(todo);
                db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(todo);
        }

        // GET: Todoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Todo todo = db.Todoes.Find(id);
            if (todo == null)
            {
                return HttpNotFound();
            }
            return View(todo);
        }

        // POST: Todoes/Edit/5
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]  // クロスサイトリクエストフォージェリ(CSRF)対策の属性。PostされてきたTokenを自動的に検証してくれる。対応するViewのフォームでも@Html.AntiForgeryToken()メソッドを呼ぶこと。
        public ActionResult Edit([Bind(Include = "Id,Summary,Detail,Limit,Done")] Todo todo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(todo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(todo);
        }

        // GET: Todoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Todo todo = db.Todoes.Find(id);
            if (todo == null)
            {
                return HttpNotFound();
            }
            return View(todo);
        }

        // POST: Todoes/Delete/5
        [HttpPost, ActionName("Delete")]  // ActionName属性を指定することでUrlのアクション名とアクションメソッド名を別名にできる。
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)  // DeleteアクションのGetとPostのメソッド名を同じにすると引数が重複してしまいエラーになるため、メソッド名を変える必要がある。
        {
            Todo todo = db.Todoes.Find(id);
            db.Todoes.Remove(todo);
            db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// 終了処理。
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            // 保持しているコンテキストを解放する。
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
