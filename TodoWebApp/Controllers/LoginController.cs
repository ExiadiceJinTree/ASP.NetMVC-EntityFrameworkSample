using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TodoWebApp.Models;

namespace TodoWebApp.Controllers
{
    /// <summary>
    /// Loginのコントローラー。
    /// コントローラー生成テンプレート"MVC5コントローラー - 空"で自動生成。
    /// 対応するViewは、本クラスのアクションメソッドを右クリック＞[ビューを追加]で別途作成。
    /// </summary>
    [AllowAnonymous]  // 本コントローラーには認証していない状態でもアクセスできるようにする。
    public class LoginController : Controller
    {
        /// <summary>
        /// ユーザー認証をするためのメンバーシッププロバイダー。
        /// </summary>
        private readonly CustomMembershipProvider _membershipProvider = new CustomMembershipProvider();

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        // 自動生成されたコントローラーにはGetアクセスのIndexアクションメソッドしかないのでPostアクセスのIndexアクションメソッドを作成。
        // POST: Login
        [HttpPost]  // Postアクションであることを指定。
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "UserName,Password")] LoginViewModel viewModel)  // 引数にはLoginViewModelを受け取る
        {
            if (ModelState.IsValid)
            {
                if (_membershipProvider.ValidateUser(viewModel.UserName, viewModel.Password))
                {
                    //====================================================================================================
                    // ユーザー名とパスワードが正しければ、
                    // 認証Cookieを設定することによって認証状態を保持するようにする。
                    //====================================================================================================
                    // FormsAuthenticationクラス: ASP.NETに用意されているWebアプリケーションのフォーム認証サービスを管理するためのクラス。これを用いて認証状態を保持する。
                    // SetAuthCookieメソッド: ユーザー名をCookieに保持する。
                    // 認証Cookieがブラウザに保持されている間は、認証状態にあると判断される。
                    FormsAuthentication.SetAuthCookie(viewModel.UserName, false);

                    // 認証処理が終了したら、
                    // TodoesControllerのIndexアクションにリダイレクト。
                    return RedirectToAction(nameof(Index), "Todoes");
                }
            }

            // 入力が正しくなかったり、認証に失敗した場合は、
            // ログイン画面に戻すようにする。
            ViewBag.Message = "ログインに失敗しました。";  // ViewBagを用いて、ログイン失敗メッセージを指定
            return View(viewModel);
        }

        // ログアウト(サインアウト)メソッド。
        public ActionResult SignOut()
        {
            // FormsAuthenticationクラスに用意されているSignOutメソッドを呼ぶことで、認証時にセットした認証Cookieを削除してくれる。
            FormsAuthentication.SignOut();

            // ログイン画面に戻す
            return RedirectToAction(nameof(Index));
        }
    }
}