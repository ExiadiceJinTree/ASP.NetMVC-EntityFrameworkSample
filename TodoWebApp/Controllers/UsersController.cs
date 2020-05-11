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
    /// Userのコントローラー。
    /// コントローラー生成テンプレート"Entity Frameworkを使用した、ビューがあるMVC5コントローラー"で自動生成。
    /// コントローラー自動生成時のオプション:
    ///   ・モデルクラス=User  ・データコンテキストクラス=TodoesContext  ・非同期コントローラーアクションの使用=false
    ///   ・ビュー:
    ///     ・ビューの生成=true  ・スクリプトライブラリの参照=true  ・レイアウトページの使用=true(~/Views/_LayoutPage1.cshtml)
    /// </summary>
    [Authorize(Roles = "Administrators")]  // ユーザー管理画面にはAdministratorsのRoleに所属するユーザーのみアクセスできるように設定。
    public class UsersController : Controller
    {
        private TodoesContext db = new TodoesContext();

        // GET: Users
        public ActionResult Index()
        {
            return View(db.Users.OrderBy(user => user.UserName).ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            this.SetRoles(new List<Role>());  // DBから全Roleを取得しリストボックス表示用に加工。
            return View();
        }

        // POST: Users/Create
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserName,Password,RoleIds")] User user)  // Bind先プロパティとしてRoleIdsを追加
        {
            // ロールListBoxに対応しているのはUser.RoleIdsプロパティ(Role.Idのリスト)のため、これを元に選択されたRoleのリストを取得。
            var selectedRoles = db.Roles.Where(role => user.RoleIds.Contains(role.Id)).ToList();

            if (ModelState.IsValid)
            {
                user.Roles = selectedRoles;  // 選択されたRoleのリストをuserパラメーターのRolesプロパティに設定

                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // ModelState.IsValid=falseの場合にも入力画面に戻るために、DBから全Roleを取得しリストボックス表示用に加工(ViewBag.RoleIdsの設定)が必要。
            // この際、リストボックス選択状態を復元するために、画面で選択されたRoleのコレクションを引数として渡す。
            this.SetRoles(selectedRoles);

            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            this.SetRoles(user.Roles);  // DBから全Roleを取得しリストボックス表示用に加工。
            return View(user);
        }

        // POST: Users/Edit/5
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserName,Password,RoleIds")] User user)  // Bind先プロパティとしてRoleIdsを追加
        {
            // ロールListBoxに対応しているのはUser.RoleIdsプロパティ(Role.Idのリスト)のため、これを元に選択されたRoleのリストを取得。
            var selectedRoles = db.Roles.Where(role => user.RoleIds.Contains(role.Id)).ToList();

            if (ModelState.IsValid)
            {
                /* [ダメなパターン1] 例外は発生しないが、Roleの変更がDBに反映されない。
                // userパラメーターを元に変更を行う。
                // userパラメーターのRolesプロパティ(null)に、選択されたRoleのリストを設定する。
                user.Roles = selectedRoles;
                db.Entry(user).State = EntityState.Modified;
                */

                /* [ダメなパターン2] ~ = EntityState.Modified行で例外(Attaching an entity of type 'TodoWebApp.Models.User' failed because another entity of the same type already has the same primary key value.)が発生。
                // userパラメーターを元に変更を行う。
                // userパラメーターのRolesプロパティ(null)に、DBから取得したUserのRolesプロパティ値をセットし、それをClearしてから選択されたRoleのリストを設定する。
                var dbUser = db.Users.Find(user.Id);
                if (dbUser == null)
                {
                    return HttpNotFound();
                }
                user.Roles = dbUser.Roles;
                user.Roles.Clear();
                user.Roles = selectedRoles;
                db.Entry(user).State = EntityState.Modified;
                */

                /* [OKのパターン1] */
                // DBから取得したUserを元に変更を行う。
                // DBのUserのRolesプロパティ値を、Clearしてから選択されたRoleのリストを設定することで、正しく更新できる。
                var dbUser = db.Users.Find(user.Id);
                if (dbUser == null)
                {
                    return HttpNotFound();
                }
                dbUser.UserName = user.UserName;
                dbUser.Password = user.Password;
                dbUser.RoleIds = user.RoleIds;
                dbUser.Roles.Clear();
                dbUser.Roles = selectedRoles;
                db.Entry(dbUser).State = EntityState.Modified;

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // ModelState.IsValid=falseの場合にも入力画面に戻るために、DBから全Roleを取得しリストボックス表示用に加工(ViewBag.RoleIdsの設定)が必要。
            // この際、リストボックス選択状態を復元するために、画面で選択されたRoleのコレクションを引数として渡す。
            this.SetRoles(selectedRoles);

            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        #region Private Method

        /// <summary>
        /// DBから全Roleを取得してリストボックスに表示できるように加工する。
        /// </summary>
        /// <param name="userRoles">ユーザーの所属ロールのコレクション。</param>
        private void SetRoles(ICollection<Role> userRoles)
        {
            var userRoleIds = userRoles.Select(userRole => userRole.Id).ToArray();

            var allRoleList = db.Roles.Select(role => new SelectListItem()
            {
                Value = role.Id.ToString(),
                Text = role.RoleName,
                Selected = userRoleIds.Contains(role.Id),  // 所属済ロールなら選択済みに設定する
            }).ToList();

            ViewBag.RoleIds = allRoleList;
        }

        #endregion
    }
}
