using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TodoWebApp.Models
{
    /// <summary>
    /// 画面に入力されたユーザー名とパスワードを本ViewModelに格納し、LoginControllerに送信する。
    /// * ViewModel: 実際のデータベースのテーブル等の項目と画面に表示する項目との違いを吸収するために使用するテクニック。Data Transfer Objectとも言う。
    /// </summary>
    public class LoginViewModel
    {
        [Required]  // 必須項目とするための属性。
        [DisplayName("ユーザー名")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]  // DataType属性をPasswordに設定することで、ログイン画面において該当のテキストボックスのタイプがパスワードになる。
        [DisplayName("パスワード")]
        public string Password { get; set; }
    }
}