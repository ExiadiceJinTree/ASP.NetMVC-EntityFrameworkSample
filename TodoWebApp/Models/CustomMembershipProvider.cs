using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Security;

namespace TodoWebApp.Models
{
    /// <summary>
    /// メンバーシップフレームワークによる認証機能を司るメンバーシッププロバイダー。
    /// * メンバーシップフレームワーク: ASP.NET2.0以降から採用された認証ライブラリで比較的シンプルに実装可能なため広く利用されている。
    /// </summary>
    public class CustomMembershipProvider : MembershipProvider  // MembershipProvider抽象クラスを継承し、抽象メソッドを実装する必要がある。
    {
        public override bool EnablePasswordRetrieval => throw new NotImplementedException();

        public override bool EnablePasswordReset => throw new NotImplementedException();

        public override bool RequiresQuestionAndAnswer => throw new NotImplementedException();

        public override string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override int MaxInvalidPasswordAttempts => throw new NotImplementedException();

        public override int PasswordAttemptWindow => throw new NotImplementedException();

        public override bool RequiresUniqueEmail => throw new NotImplementedException();

        public override MembershipPasswordFormat PasswordFormat => throw new NotImplementedException();

        public override int MinRequiredPasswordLength => throw new NotImplementedException();

        public override int MinRequiredNonAlphanumericCharacters => throw new NotImplementedException();

        public override string PasswordStrengthRegularExpression => throw new NotImplementedException();

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 本アプリで実装する機能。認証機能を司るメソッド。
        /// データ ソースに指定したユーザー名とパスワードが存在することを確認します。
        /// </summary>
        /// <param name="username">検証するユーザーの名前。</param>
        /// <param name="password">指定したユーザーのパスワード。</param>
        /// <returns>指定されたユーザー名とパスワードが有効な場合は、true。それ以外の場合は、false。</returns>
        public override bool ValidateUser(string username, string password)
        {
            using (var db = new TodoesContext())  // コンテキストクラスを介してDBへの操作を行う
            {
                string pwdHash = this.GeneratePasswordHash(username, password);  // パスワードをハッシュ化

                var user = db.Users
                             .Where(u => u.UserName == username && u.Password == pwdHash)  // ハッシュ化されたパスワード同士で比較
                             .FirstOrDefault();  // 最初に見つかった1件を返却、見つからなければdefault値(classなのでnull)を返す。
                if (user == null)
                {
                    return false;
                }
                return true;
            }
        }


        /// <summary>
        /// 指定のユーザー名とパスワードを元に、Hash化されたパスワードを生成し返す。
        /// </summary>
        /// <param name="userName">ユーザー名。</param>
        /// <param name="password">パスワード。</param>
        /// <returns>パスワードハッシュ値。</returns>
        public string GeneratePasswordHash(string userName, string password)
        {
            //====================================================================================================
            // saltを作成
            // * salt(ソルト): パスワードに付加する、文字列長をかさ増しするための、ユーザー毎に異なるある程度(20桁以上)の長さの文字列。
            // * 簡単なsaltの生成方法として、ユーザーID/名のHash値がある。
            //====================================================================================================
            // saltの元文字列を生成(本アプリではユーザー名を短くできるので、長くするために接頭辞"secret_"+userNameとする)
            string rawSalt = $"secret_{userName}";

            // Hash化(SHA256方式)したsaltを生成
            var sha256CryptoSvcProvider = new SHA256CryptoServiceProvider();
            byte[] saltBytes = sha256CryptoSvcProvider.ComputeHash(System.Text.Encoding.UTF8.GetBytes(rawSalt));

            //====================================================================================================
            // PBKDF2(Password-Based Key Derivation Function 2)アルゴリズムによるパスワードのHash化
            // * PBKDF2: Hash化、salt、ストレッチングを組み合わせたパスワード保護方式。RFC2898として提案された。
            //   * ストレッチング: Hash関数を繰り返し適用することで、パスワードをより強力に保護する方法。1万回以上繰り返すのが推奨される。
            //====================================================================================================
            var deriveBytes = new Rfc2898DeriveBytes(password, saltBytes, 10000);  // 第3引数は、ストレッチングのための演算の反復処理回数。
            byte[] passwordHashBytes = deriveBytes.GetBytes(32);  // 擬似ランダムキー(バイト配列)を生成。

            //====================================================================================================
            // パスワードハッシュバイト配列をBase64形式の文字列に変換
            //====================================================================================================
            // * Base64: データを64種類の印字可能な英数字のみを用いて、それ以外の文字を扱うことの出来ない通信環境にてマルチバイト文字やバイナリデータを扱うためのエンコード方式。
            string passwordHashStr = Convert.ToBase64String(passwordHashBytes);

            return passwordHashStr;
        }
    }
}