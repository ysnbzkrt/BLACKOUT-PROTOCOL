# 🕵️‍♂️ Blackout Protocol — Technical System & Architecture Overview

> **Engine:** Unity 6 (6000.0.3f1) | **Language:** C# (.NET Core / Mono) | **Architecture:** Component-Based & Event-Driven

**Blackout Protocol**, izometrik (top-down) bakış açısına sahip; yapay zeka görüş algoritmaları, etkileşimli nesne mekanikleri, dynamic audio routing ve state machine tabanlı oyun döngülerine dayalı bir gizlilik-aksiyon (stealth) projesidir.

---

## 👨‍💻 Geliştirici Bilgisi

| Adı Soyadı | İletişim / E-Posta | Rol & Sorumluluk |
| :--- | :--- | :--- |
| **Yasin Bozkurt** | [yasinbozkurt068@gmail.com](mailto:yasinbozkurt068@gmail.com) | Solo Developer (Game Logic, Raycast/FOV AI Engine, Dynamic Audio Mixer Architecture, Interactive Systems & UI Management) |

---

## 🎯 Teknik Mimari ve Sorumlu Sistemler

### 1. 👁️ Düşman Görüş ve Tespit Sistemi (Raycast & FOV Engine)
Sistem, performans optimizasyonu için her karede çalışan `Update` yerine belirli frekanslarda çalışan bir algılama döngüsüne (`OverlapSphere` & `Raycast`) dayanır.

* **Field of View (FOV) Açı & Mesafe Kontrolü:** Düşman, oyuncunun pozisyonunu kendi `forward` vektörü ile karşılaştırarak açısal farkı (`Vector3.Angle`) hesaplar.
* **Line of Sight (LOS) Raycasting:** Oyuncu görüş açısına girdiğinde, arada duvar/engel olup olmadığını doğrulamak için `Physics.Raycast` atılır. 
* **Detection & Alert Routing:** Engel yoksa tespit gerçekleşir, tek seferlik uyarı sesi (`AudioSource.PlayOneShot`) tetiklenir ve AI takip durumuna geçer.

### 2. 🤺 Dinamik Düşman Saldırı ve Oyuncu Tepki Animasyonları (Context-Aware Animation System)
Oyun içerisindeki savaş ve etkileşim hissini artırmak için statik animasyonlar yerine düşman tipine göre dinamikleşen bir Animator Controller yapısı kurgulanmıştır.

* **Enemy-Specific Attack Logic:** Her düşman tipi kendi Animator Controller bileşeninde farklı bir vurma (Attack) animasyon trigger'ına sahiptir.
* **Dynamic Player Death Responses:** Oyuncunun aldığı ölüm animasyonu, kendisini öldüren düşmanın türüne/saldırı tipine (örneğin yakın dövüş veya ateşli silah) göre dinamik olarak belirlenir ve ilgili ragdoll/death state tetiklenir.

### 3. 💾 Toplanabilir Diskler & Etkileşimli Terminal / Sahne Geçiş Sistemi
Gizlilik ve hacking temasını destekleyen görev mekanikleri `IInteractable` mantığıyla modüler hale getirilmiştir.

* **Collectible Disks (Data Drives):** Bölgeye dağıtılmış veri diskleri, oyuncunun envanter/görev durumunu güncelleyen trigger alanları ile toplanır.
* **Interactive PC Terminals & Scene Flow:** Oyuncu gerekli diskleri topladıktan sonra etkileşimli bilgisayar terminaline yaklaştığında ekranda etkileşim uyarısı belirir. **'E' tuşuna** basıldığında terminal hacklenir ve bir sonraki sahneye (`SceneManager.LoadScene`) güvenli geçiş sağlanır.

### 4. 🎛️ Dynamic Audio Mixer & Logaritmik Desibel Dönüşümü
Oyun içi ses efektleri (SFX) ve arka plan müzikleri (Music) bağımsız kanallarda işlenir. 

* **Logarithmic Volume Attenuation:** Linear Slider değerleri (0.0001 - 1.0) doğrudan Audio Mixer'a verilmez; insan kulağının ses algısına uygun şekilde $dB = 20 \times \log_{10}(\text{SliderValue})$ formülü ile desibele dönüştürülür.
* **Exposed Parameter Mapping:** Runtime sırasında `musicVol` ve `sfxVol` parametreleri doğrudan script üzerinden kontrol edilir.
* **Data Persistence:** Ses tercihleri `PlayerPrefs` ile yerel depolamada saklanır ve sahne yüklenmelerinde `Awake` / `Start` fazında otomatik yüklenir.

### 5. ⚡ Laser Hazards & Death Sequence Pipeline
* **Trigger Based Detection:** Lazer bariyerlerine eklenen `OnTriggerEnter` olayları, oyuncunun ölme durumunu tetikler.
* **Camera Zoom & Time Scale:** Ölüm anında kamera odağı oyuncuya kilitlenerek zoom efekti uygulanır; oyun içi zaman durdurulur (`Time.timeScale = 0`).
* **Game Over Event Flow:** Oyuncu öldüğü an tüm AI görüş scriptleri pasife alınarak ses tekrarı veya mantık hataları engellenir.

---

## 🛠️ Teknik Bilgiler ve Bağımlılıklar

* **Unity Version:** Unity 6 (`6000.0.3f1`)
* **Physics & Interactions:** Unity 3D Physics (Raycasting, Triggers, Layer Masking), Scene Management
* **Audio System:** Master Audio Mixer, Multi-Channel Bus Management, PlayerPrefs Integration
* **Version Control:** Git & GitHub (Git LFS configured for large mesh Assets)

---

## 🕹️ Kurulum ve Çalıştırma (Playable Build)

1. **[Blackout Protocol v1.0.0 (Windows .exe) İndir](https://github.com/ysnbzkrt/BLACKOUT-PROTOCOL/releases/latest)**
2. `.zip` dosyasını bir klasöre çıkartın.
3. `Blackout Protocol.exe` dosyasını çalıştırın.

### 💻 Projeyi Unity Editor'de Çalıştırma
```bash
git clone [https://github.com/ysnbzkrt/BLACKOUT-PROTOCOL.git](https://github.com/ysnbzkrt/BLACKOUT-PROTOCOL.git)
