using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UnityKit {
    public class ISO6391 {
        public struct Lang {
            public readonly string name;
            public readonly string code;
            public readonly SystemLanguage lang;

            public Lang(SystemLanguage lang, string name, string code) {
                this.lang = lang;
                this.name = name;
                this.code = code;
            }

            public override bool Equals(object obj) {
                if (ReferenceEquals(null, obj)) return false;
                else return ReferenceEquals(this, obj);
            }

            public override string ToString() { return string.Format("ISO369.1 EnglishName:{0} CodeName:{1}", name, code); }
            public override int GetHashCode() { return lang.GetHashCode() + code.GetHashCode(); }

            static public bool operator ==(Lang a, Lang b) { return a.lang == b.lang; }
            static public bool operator !=(Lang a, Lang b) { return a.lang == b.lang; }
        }

        //----------------------------------------
        static public readonly Lang Afrikaans = new Lang(SystemLanguage.Afrikaans, "Afrikaans", "af");
        static public readonly Lang Arabic = new Lang(SystemLanguage.Arabic, "Arabic", "ar");
        static public readonly Lang Basque = new Lang(SystemLanguage.Basque, "Basque", "eu");
        static public readonly Lang Belarusian = new Lang(SystemLanguage.Belarusian, "Belarusian", "be");
        static public readonly Lang Bulgarian = new Lang(SystemLanguage.Bulgarian, "Bulgarian", "bg");
        static public readonly Lang Catalan = new Lang(SystemLanguage.Catalan, "Catalan", "ca");
        static public readonly Lang Chinese = new Lang(SystemLanguage.Chinese, "Chinese", "zh");
        static public readonly Lang Czech = new Lang(SystemLanguage.Czech, "Czech", "cs");
        static public readonly Lang Danish = new Lang(SystemLanguage.Danish, "Danish", "da");
        static public readonly Lang Dutch = new Lang(SystemLanguage.Dutch, "Dutch", "nl");
        static public readonly Lang English = new Lang(SystemLanguage.English, "English", "en");
        static public readonly Lang Estonian = new Lang(SystemLanguage.Estonian, "Estonian", "et");
        static public readonly Lang Faroese = new Lang(SystemLanguage.Faroese, "Faroese", "fo");
        static public readonly Lang Finnish = new Lang(SystemLanguage.Finnish, "Finnish", "fi");
        static public readonly Lang French = new Lang(SystemLanguage.French, "French", "fr");
        static public readonly Lang German = new Lang(SystemLanguage.German, "German", "de");
        static public readonly Lang Greek = new Lang(SystemLanguage.Greek, "Greek", "el");
        static public readonly Lang Hebrew = new Lang(SystemLanguage.Hebrew, "Hebrew", "he");
        static public readonly Lang Hungarian = new Lang(SystemLanguage.Hungarian, "Hungarian", "hu");
        static public readonly Lang Icelandic = new Lang(SystemLanguage.Icelandic, "Icelandic", "is");
        static public readonly Lang Indonesian = new Lang(SystemLanguage.Indonesian, "Indonesian", "id");
        static public readonly Lang Italian = new Lang(SystemLanguage.Italian, "Italian", "it");
        static public readonly Lang Japanese = new Lang(SystemLanguage.Japanese, "Japanese", "ja");
        static public readonly Lang Korean = new Lang(SystemLanguage.Korean, "Korean", "ko");
        static public readonly Lang Latvian = new Lang(SystemLanguage.Latvian, "Latvian", "lv");
        static public readonly Lang Lithuanian = new Lang(SystemLanguage.Lithuanian, "Lithuanian", "lt");
        static public readonly Lang Norwegian = new Lang(SystemLanguage.Norwegian, "Norwegian", "nb");
        static public readonly Lang Polish = new Lang(SystemLanguage.Polish, "Polish", "pl");
        static public readonly Lang Portuguese = new Lang(SystemLanguage.Portuguese, "Portuguese", "pt");
        static public readonly Lang Romanian = new Lang(SystemLanguage.Romanian, "Romanian", "ro");
        static public readonly Lang Russian = new Lang(SystemLanguage.Russian, "Russian", "ru");
        static public readonly Lang SerboCroatian = new Lang(SystemLanguage.SerboCroatian, "SerboCroatian", "sr-BA");
        static public readonly Lang Slovak = new Lang(SystemLanguage.Slovak, "Slovak", "sk");
        static public readonly Lang Slovenian = new Lang(SystemLanguage.Slovenian, "Slovenian", "sl");
        static public readonly Lang Spanish = new Lang(SystemLanguage.Spanish, "Spanish", "es");
        static public readonly Lang Swedish = new Lang(SystemLanguage.Swedish, "Swedish", "sv");
        static public readonly Lang Thai = new Lang(SystemLanguage.Thai, "Thai", "th");
        static public readonly Lang Turkish = new Lang(SystemLanguage.Turkish, "Turkish", "tr");
        static public readonly Lang Ukrainian = new Lang(SystemLanguage.Ukrainian, "Ukrainian", "uk");
        static public readonly Lang Vietnamese = new Lang(SystemLanguage.Vietnamese, "Vietnamese", "vi");
        static public readonly Lang ChineseSimplified = new Lang(SystemLanguage.ChineseSimplified, "ChineseSimplified", "zh-CN");
        static public readonly Lang ChineseTraditional = new Lang(SystemLanguage.ChineseTraditional, "ChineseTraditional", "zh-TW");
        static public readonly Lang Unknown = new Lang(SystemLanguage.Unknown, "", "");

        static public readonly Lang[] ALL = new Lang[] { Afrikaans, Arabic, Basque, Belarusian, Bulgarian, Catalan, Chinese, Czech, Danish, Dutch, English, Estonian, Faroese, Finnish, French, German, Greek, Hebrew, Hungarian, Icelandic, Indonesian, Italian, Japanese, Korean, Latvian, Lithuanian, Norwegian, Polish, Portuguese, Romanian, Russian, SerboCroatian, Slovak, Slovenian, Spanish, Swedish, Thai, Turkish, Ukrainian, Vietnamese, ChineseSimplified, ChineseTraditional };
        static public readonly string[] AllCode = new string[] { "af", "ar", "eu", "be", "bg", "ca", "zh", "cs", "da", "nl", "en", "et", "fo", "fi", "fr", "de", "el", "he", "hu", "is", "id", "it", "ja", "ko", "lv", "lt", "nb", "pl", "pt", "ro", "ru", "sr-BA", "sk", "sl", "es", "sv", "th", "tr", "uk", "vi", "zh-CN", "zh-TW", };
        static public readonly string[] AllLangName = new string[] { "Afrikaans", "Arabic", "Basque", "Belarusian", "Bulgarian", "Catalan", "Chinese", "Czech", "Danish", "Dutch", "English", "Estonian", "Faroese", "Finnish", "French", "German", "Greek", "Hebrew", "Hungarian", "Icelandic", "Indonesian", "Italian", "Japanese", "Korean", "Latvian", "Lithuanian", "Norwegian", "Polish", "Portuguese", "Romanian", "Russian", "SerboCroatian", "Slovak", "Slovenian", "Spanish", "Swedish", "Thai", "Turkish", "Ukrainian", "Vietnamese", "ChineseSimplified", "ChineseTraditional" };
        static public bool IsCode(string str) { return AllCode.Contains(str); }
        static public bool IsLangName(string str) { return AllLangName.Contains(str); }

        static public string GetCode(string name) {
            if (name == "Afrikaans") return "af";
            if (name == "Arabic") return "ar";
            if (name == "Basque") return "eu";
            if (name == "Belarusian") return "be";
            if (name == "Bulgarian") return "bg";
            if (name == "Catalan") return "ca";
            if (name == "Chinese") return "zh";
            if (name == "Czech") return "cs";
            if (name == "Danish") return "da";
            if (name == "Dutch") return "nl";
            if (name == "English") return "en";
            if (name == "Estonian") return "et";
            if (name == "Faroese") return "fo";
            if (name == "Finnish") return "fi";
            if (name == "French") return "fr";
            if (name == "German") return "de";
            if (name == "Greek") return "el";
            if (name == "Hebrew") return "he";
            if (name == "Hungarian") return "hu";
            if (name == "Icelandic") return "is";
            if (name == "Indonesian") return "id";
            if (name == "Italian") return "it";
            if (name == "Japanese") return "ja";
            if (name == "Korean") return "ko";
            if (name == "Latvian") return "lv";
            if (name == "Lithuanian") return "lt";
            if (name == "Norwegian") return "nb";
            if (name == "Polish") return "pl";
            if (name == "Portuguese") return "pt";
            if (name == "Romanian") return "ro";
            if (name == "Russian") return "ru";
            if (name == "SerboCroatian") return "sr-BA";
            if (name == "Slovak") return "sk";
            if (name == "Slovenian") return "sl";
            if (name == "Spanish") return "es";
            if (name == "Swedish") return "sv";
            if (name == "Thai") return "th";
            if (name == "Turkish") return "tr";
            if (name == "Ukrainian") return "uk";
            if (name == "Vietnamese") return "vi";
            if (name == "ChineseSimplified") return "zh-CN";
            if (name == "ChineseTraditional") return "zh-TW";
            return AllCode.Contains(name) ? name : "";
        }

        static public string GetCode(SystemLanguage lang) {
            if (lang == SystemLanguage.Afrikaans) return "af";
            if (lang == SystemLanguage.Arabic) return "ar";
            if (lang == SystemLanguage.Basque) return "eu";
            if (lang == SystemLanguage.Belarusian) return "be";
            if (lang == SystemLanguage.Bulgarian) return "bg";
            if (lang == SystemLanguage.Catalan) return "ca";
            if (lang == SystemLanguage.Chinese) return "zh";
            if (lang == SystemLanguage.Czech) return "cs";
            if (lang == SystemLanguage.Danish) return "da";
            if (lang == SystemLanguage.Dutch) return "nl";
            if (lang == SystemLanguage.English) return "en";
            if (lang == SystemLanguage.Estonian) return "et";
            if (lang == SystemLanguage.Faroese) return "fo";
            if (lang == SystemLanguage.Finnish) return "fi";
            if (lang == SystemLanguage.French) return "fr";
            if (lang == SystemLanguage.German) return "de";
            if (lang == SystemLanguage.Greek) return "el";
            if (lang == SystemLanguage.Hebrew) return "he";
            if (lang == SystemLanguage.Hungarian) return "hu";
            if (lang == SystemLanguage.Icelandic) return "is";
            if (lang == SystemLanguage.Indonesian) return "id";
            if (lang == SystemLanguage.Italian) return "it";
            if (lang == SystemLanguage.Japanese) return "ja";
            if (lang == SystemLanguage.Korean) return "ko";
            if (lang == SystemLanguage.Latvian) return "lv";
            if (lang == SystemLanguage.Lithuanian) return "lt";
            if (lang == SystemLanguage.Norwegian) return "nb";
            if (lang == SystemLanguage.Polish) return "pl";
            if (lang == SystemLanguage.Portuguese) return "pt";
            if (lang == SystemLanguage.Romanian) return "ro";
            if (lang == SystemLanguage.Russian) return "ru";
            if (lang == SystemLanguage.SerboCroatian) return "sr-BA";
            if (lang == SystemLanguage.Slovak) return "sk";
            if (lang == SystemLanguage.Slovenian) return "sl";
            if (lang == SystemLanguage.Spanish) return "es";
            if (lang == SystemLanguage.Swedish) return "sv";
            if (lang == SystemLanguage.Thai) return "th";
            if (lang == SystemLanguage.Turkish) return "tr";
            if (lang == SystemLanguage.Ukrainian) return "uk";
            if (lang == SystemLanguage.Vietnamese) return "vi";
            if (lang == SystemLanguage.ChineseSimplified) return "zh-CN";
            if (lang == SystemLanguage.ChineseTraditional) return "zh-TW";
            return "";
        }

        static public string GetName(string code) {
            if (code == "af") return "Afrikaans";
            if (code == "ar") return "Arabic";
            if (code == "eu") return "Basque";
            if (code == "be") return "Belarusian";
            if (code == "bg") return "Bulgarian";
            if (code == "ca") return "Catalan";
            if (code == "zh") return "Chinese";
            if (code == "cs") return "Czech";
            if (code == "da") return "Danish";
            if (code == "nl") return "Dutch";
            if (code == "en") return "English";
            if (code == "et") return "Estonian";
            if (code == "fo") return "Faroese";
            if (code == "fi") return "Finnish";
            if (code == "fr") return "French";
            if (code == "de") return "German";
            if (code == "el") return "Greek";
            if (code == "he") return "Hebrew";
            if (code == "hu") return "Hungarian";
            if (code == "is") return "Icelandic";
            if (code == "id") return "Indonesian";
            if (code == "it") return "Italian";
            if (code == "ja") return "Japanese";
            if (code == "ko") return "Korean";
            if (code == "lv") return "Latvian";
            if (code == "lt") return "Lithuanian";
            if (code == "nb") return "Norwegian";
            if (code == "pl") return "Polish";
            if (code == "pt") return "Portuguese";
            if (code == "ro") return "Romanian";
            if (code == "ru") return "Russian";
            if (code == "sr-BA") return "SerboCroatian";
            if (code == "sk") return "Slovak";
            if (code == "sl") return "Slovenian";
            if (code == "es") return "Spanish";
            if (code == "sv") return "Swedish";
            if (code == "th") return "Thai";
            if (code == "tr") return "Turkish";
            if (code == "uk") return "Ukrainian";
            if (code == "vi") return "Vietnamese";
            if (code == "zh-CN") return "ChineseSimplified";
            if (code == "zh-TW") return "ChineseTraditional";
            return AllLangName.Contains(code) ? code : "";
        }

        static public string GetName(SystemLanguage language) {
            if (language == SystemLanguage.Unknown) return "";
            return language.ToString();
        }

        static public Lang GetLang(SystemLanguage syslang) {
            if (syslang == SystemLanguage.Afrikaans) return Afrikaans;
            if (syslang == SystemLanguage.Arabic) return Arabic;
            if (syslang == SystemLanguage.Basque) return Basque;
            if (syslang == SystemLanguage.Belarusian) return Belarusian;
            if (syslang == SystemLanguage.Bulgarian) return Bulgarian;
            if (syslang == SystemLanguage.Catalan) return Catalan;
            if (syslang == SystemLanguage.Chinese) return Chinese;
            if (syslang == SystemLanguage.Czech) return Czech;
            if (syslang == SystemLanguage.Danish) return Danish;
            if (syslang == SystemLanguage.Dutch) return Dutch;
            if (syslang == SystemLanguage.English) return English;
            if (syslang == SystemLanguage.Estonian) return Estonian;
            if (syslang == SystemLanguage.Faroese) return Faroese;
            if (syslang == SystemLanguage.Finnish) return Finnish;
            if (syslang == SystemLanguage.French) return French;
            if (syslang == SystemLanguage.German) return German;
            if (syslang == SystemLanguage.Greek) return Greek;
            if (syslang == SystemLanguage.Hebrew) return Hebrew;
            if (syslang == SystemLanguage.Hungarian) return Hungarian;
            if (syslang == SystemLanguage.Icelandic) return Icelandic;
            if (syslang == SystemLanguage.Indonesian) return Indonesian;
            if (syslang == SystemLanguage.Italian) return Italian;
            if (syslang == SystemLanguage.Japanese) return Japanese;
            if (syslang == SystemLanguage.Korean) return Korean;
            if (syslang == SystemLanguage.Latvian) return Latvian;
            if (syslang == SystemLanguage.Lithuanian) return Lithuanian;
            if (syslang == SystemLanguage.Norwegian) return Norwegian;
            if (syslang == SystemLanguage.Polish) return Polish;
            if (syslang == SystemLanguage.Portuguese) return Portuguese;
            if (syslang == SystemLanguage.Romanian) return Romanian;
            if (syslang == SystemLanguage.Russian) return Russian;
            if (syslang == SystemLanguage.SerboCroatian) return SerboCroatian;
            if (syslang == SystemLanguage.Slovak) return Slovak;
            if (syslang == SystemLanguage.Slovenian) return Slovenian;
            if (syslang == SystemLanguage.Spanish) return Spanish;
            if (syslang == SystemLanguage.Swedish) return Swedish;
            if (syslang == SystemLanguage.Thai) return Thai;
            if (syslang == SystemLanguage.Turkish) return Turkish;
            if (syslang == SystemLanguage.Ukrainian) return Ukrainian;
            if (syslang == SystemLanguage.Vietnamese) return Vietnamese;
            if (syslang == SystemLanguage.ChineseSimplified) return ChineseSimplified;
            if (syslang == SystemLanguage.ChineseTraditional) return ChineseTraditional;
            return Unknown;
        }

        static public Lang GetLang(string langstr) {
            if (langstr == "af" || langstr == "Afrikaans") return Afrikaans;
            if (langstr == "ar" || langstr == "Arabic") return Arabic;
            if (langstr == "eu" || langstr == "Basque") return Basque;
            if (langstr == "be" || langstr == "Belarusian") return Belarusian;
            if (langstr == "bg" || langstr == "Bulgarian") return Bulgarian;
            if (langstr == "ca" || langstr == "Catalan") return Catalan;
            if (langstr == "zh" || langstr == "Chinese") return Chinese;
            if (langstr == "cs" || langstr == "Czech") return Czech;
            if (langstr == "da" || langstr == "Danish") return Danish;
            if (langstr == "nl" || langstr == "Dutch") return Dutch;
            if (langstr == "en" || langstr == "English") return English;
            if (langstr == "et" || langstr == "Estonian") return Estonian;
            if (langstr == "fo" || langstr == "Faroese") return Faroese;
            if (langstr == "fi" || langstr == "Finnish") return Finnish;
            if (langstr == "fr" || langstr == "French") return French;
            if (langstr == "de" || langstr == "German") return German;
            if (langstr == "el" || langstr == "Greek") return Greek;
            if (langstr == "he" || langstr == "Hebrew") return Hebrew;
            if (langstr == "hu" || langstr == "Hungarian") return Hungarian;
            if (langstr == "is" || langstr == "Icelandic") return Icelandic;
            if (langstr == "id" || langstr == "Indonesian") return Indonesian;
            if (langstr == "it" || langstr == "Italian") return Italian;
            if (langstr == "ja" || langstr == "Japanese") return Japanese;
            if (langstr == "ko" || langstr == "Korean") return Korean;
            if (langstr == "lv" || langstr == "Latvian") return Latvian;
            if (langstr == "lt" || langstr == "Lithuanian") return Lithuanian;
            if (langstr == "nb" || langstr == "Norwegian") return Norwegian;
            if (langstr == "pl" || langstr == "Polish") return Polish;
            if (langstr == "pt" || langstr == "Portuguese") return Portuguese;
            if (langstr == "ro" || langstr == "Romanian") return Romanian;
            if (langstr == "ru" || langstr == "Russian") return Russian;
            if (langstr == "sr-BA" || langstr == "SerboCroatian") return SerboCroatian;
            if (langstr == "sk" || langstr == "Slovak") return Slovak;
            if (langstr == "sl" || langstr == "Slovenian") return Slovenian;
            if (langstr == "es" || langstr == "Spanish") return Spanish;
            if (langstr == "sv" || langstr == "Swedish") return Swedish;
            if (langstr == "th" || langstr == "Thai") return Thai;
            if (langstr == "tr" || langstr == "Turkish") return Turkish;
            if (langstr == "uk" || langstr == "Ukrainian") return Ukrainian;
            if (langstr == "vi" || langstr == "Vietnamese") return Vietnamese;
            if (langstr == "zh-CN" || langstr == "ChineseSimplified") return ChineseSimplified;
            if (langstr == "zh-TW" || langstr == "ChineseTraditional") return ChineseTraditional;
            return Unknown;
        }
    }
}
