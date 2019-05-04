# CryptBlog
<<<<<<< HEAD
Project of BSc subject Cryptography

##PROJE ADIM-1
Proje klasörü içindeki "CryptBlog.sln" dosyasını açarak Visual Studio üzerinde çalıştırabilirsiniz.
Veritabanı için ödev kontrolünü kolaylaştırmak açısından SQLLite kullanılmıştır. ("test.db" isimli dosya veritabanı dosyasıdır.)
Veritabanı için herhangi bir import işlemi yapmanıza gerek yoktur. Doğrudan projeyi çalıştırıp incelemeye başlayabilirsiniz.

##PROJE ADIM-2
-Bu adım için AES şifreleme algoritması seçilmiştir.
-Veritabanı tablosu üzerinde değişiklikler yapılmıştır.
-Tablo artık ayrıca blog içeriklerinin şifreli hali, şifreleyen key ve initial vector tutulmaktadır.
-Models/EncryptionModel.cs 'de projede kullanılabilecek ileriye dönük metotlar bulunmaktadır.
-AES algoritmasının gereksinimi üzerine key en az 24 karakterli olmalıdır.
-Proje şifresiz blog yazılarını "OnurCryptKey000000000000" key'iyle şifrelemektedir.(ContentModel.cs/EncryptAll())
-EncryptionModel objesi üzerinden şifrelenmemiş ContentEntity objeleri(encryptedBody property'si boş objeler)
'AES_EncryptContentEntity'e bir key ile gönderilir ve geriye dönen ContentEntity içerisinde key(tip:byte[]) ve initial vector(tip:byte[]) eklenir.
-Ayrıca ContentModel.cs içerisine 'InsertWithAESEncryption' metotu yerleştirilmiştir. Veritabanına insert işlemi yapmak için title,body ve key alan bu metot verilen key'e göre
şifreler ve veritabanına insert eder.
-EncryptionModel.cs üzerinde decrypt metotu da bulunmaktadır.

**5.Senaryo yorum:**
  *Aes algoritmasının çalışma prensibi ve InitialVector yardımıyla salting işlemi yapıldığı için(tekrar eden kelimeler vb. durumların yarattığı
  'dictionary attack' zaafiyetini ortadan kaldırmak için her şifrelemede rastgele oluşturulup saklanmaktadır) senaryo-5.1 ve senaryo-5.2 arasında
  zaafiyete sebep olacak herhangi bir benzerlik, tekrar eden bir kalıp vs. görülmemiştir.
NOT:Tarayıcınızın geliştirici konsolu üzerinden(ör: Chrome'da f12) 'contents' javascript objesine ulaşıp keşif yapabilirsiniz.:) Örneğin: Key byte array olarak saklanmakta
sadece görüntülerken base64String'e çevrilmektedir. (javascript function:> atob())*

##PROJE ADIM-3
Kullanım:
-'RandomCommentModel.cs' class'ı .net Cryptography namespace'i içindeki RNGCryptoServiceProvider'ı kullanarak
100 karater uzunluğunda random string oluşturur. Aynı zamanda karakterlerin dağılımını hesaplar.
-Proje'nin herhangi bir yerinde bu class yardımıyla random string oluşturulabilir. Örnek kullanım dahilinde; HomeController içinde
AddRandomComment methoduyla random oluşturulmuş ve commentModel class'ıyla veritabanına yazılmıştır.
-Veritabanında yorum: [ID,ContentId,Title,Body,Analysis] şeklinde tutulmaktadır. contentId ait olduğu blog postu title: bu örnek dahilinde random string,
body: string'in açıklaması, Analysis: frekans dağılımının json objesi halinde tutumunu sağlar.

**Yorum:**
  *Standart C# random class'ının CPU clock'u seed olarak kullanarak tahmin edilebilir olmasına karşın,
  RNGCryptoServiceProvider işletim sisteminin 'Entropy'siyle tamamen rastgele bir seed ile rastsal bir sayı oluşturarak iyileştirme sağlandı.
  Proje üzerinde herhangi bir blog girdisi altına random yorum girme butonu eklendi. Böylece CPU saatinden bağımsız rastsallık test edilebilir.
  Yorum'un ayrıntılarına tıkladığınızda açılan modal'da random stringin karakter dağılım grafiğini görebilirsiniz.*

##PROJE ADIM-4
Kullanım:
-İmzalar veritabanında 'SignatureEntity' [Id,Description,PublicKey,PrivateKey] şeklinde tutulmaktadır.
-'SignatureModel.cs' üzerinden
  *-Tüm imzalar alınabilir, güvenlik zaafiyeti oluşmaması için privateKey içermeyen 'SelectAllOnlyPublicKey' methodu ile de imzalar alınabilir.
  *'CreateNewSignature' methoduna yeni oluşturulması istenen imza için açıklama gönderilir, method imza oluşturur ve veritabanına kaydeder.
  *public olan 'SignComment' methoduna imzalanmak için kullanılacak imzanın signatureId'si ve imzalanacak yorumun commentId'si gönderilir,
   method class içindeki private 'SignString' methoduyla imzalamayı yapar, imzalanmış veriyi ve hangi imza ile imzalandığı(signatureId)
   bilgisini veritabanındaki yorumu günceller.(Yorumların tutulduğu 'CommentEntity' tablosunda imzalanacak yoruma ait 'SignedBy' ve 'SignedData')
  *public olan 'VerifyComment' methoduna doğrulanması istenen yorum'un Id'si ve doğrulama için kullanılacak publicKey gönderilir,
   method class içindeki private 'VerifyString' methoduyla doğrulama yapar, doğrulama sonucunu geriye döndürür(bool)
   
**Test Senaryosu:**
-Projede hali hazırda 'Onur Yurteri' ve 'Bob Ross' isimli 2 imza oluşturulmuştur. Yeni imza SignatureModel class'ı üzerinden
CreateNewSignature ile oluşturulabilir.
-Yorum Id'si 1 ve 4 olan yorumlar(Senaryo-1 altındaki ilk 2 yorum), 1:'Onur Yurteri', 2:'Bob Ross' tarafından imzalanmıştır.
-Herhangi bir yorum'un ayrıntılarına tıklanır. Eğer yorum imzalanmışsa açılan modal'da; kim tarafından imzalandığı bilgisi, imzalayan imzanın public key'i
görüntülenecektir. İmzalı olmayan yorumlarda bu bilgiler doğal olarak gözükmemektedir.
-Ayrıca 'SignatureModel' üzerindeki 'VerifyComment' methodu yardımıyla doğrulama testi için kullanıcının public key girebileceği input bulunmaktadır.
kullanıcı public key girebilir ve doğrulama yapabilir.
=======
Project of BSc subject Cryptography, Spring 2019 
>>>>>>> c0e4d8536984869f0f8aead4d42a1d85f7126136
