/**
 * http://www.lingoes.net/en/translator/langcode.htm
 * 采用Unity SystemLanguage 方案实现
 * 并非完整的语言实现。并且有可能读取到不到系统语言的情况
 * 考虑使用Navtive 的方案实现完整的多语言
 */
using System.Collections.Generic;
using UnityEngine;

namespace UnityKit {

    sealed public class Language {

        public readonly SystemLanguage language;
        public readonly string name;
        public readonly string code;

        Language(SystemLanguage language, string name, string code) {
            this.language = language;
            this.name = name;
            this.code = code;
        }

        static public readonly Language Afrikaans = new Language(SystemLanguage.Afrikaans, "Afrikaans", "af");
        static public readonly Language Arabic = new Language(SystemLanguage.Arabic, "Arabic", "ar");
        static public readonly Language Basque = new Language(SystemLanguage.Basque, "Basque", "eu");
        static public readonly Language Belarusian = new Language(SystemLanguage.Belarusian, "Belarusian", "be");
        static public readonly Language Bulgarian = new Language(SystemLanguage.Bulgarian, "Bulgarian", "bg");
        static public readonly Language Catalan = new Language(SystemLanguage.Catalan, "Catalan", "ca");
        static public readonly Language Chinese = new Language(SystemLanguage.Chinese, "Chinese", "zh");
        static public readonly Language Czech = new Language(SystemLanguage.Czech, "Czech", "cs");
        static public readonly Language Danish = new Language(SystemLanguage.Danish, "Danish", "da");
        static public readonly Language Dutch = new Language(SystemLanguage.Dutch, "Dutch", "nl");
        static public readonly Language English = new Language(SystemLanguage.English, "English", "en");
        static public readonly Language Estonian = new Language(SystemLanguage.Estonian, "Estonian", "et");
        static public readonly Language Faroese = new Language(SystemLanguage.Faroese, "Faroese", "fo");
        static public readonly Language Finnish = new Language(SystemLanguage.Finnish, "Finnish", "fi");
        static public readonly Language French = new Language(SystemLanguage.French, "French", "fr");
        static public readonly Language German = new Language(SystemLanguage.German, "German", "de");
        static public readonly Language Greek = new Language(SystemLanguage.Greek, "Greek", "el");
        static public readonly Language Hebrew = new Language(SystemLanguage.Hebrew, "Hebrew", "he");
        static public readonly Language Hungarian = new Language(SystemLanguage.Hungarian, "Hungarian", "hu");
        static public readonly Language Icelandic = new Language(SystemLanguage.Icelandic, "Icelandic", "is");
        static public readonly Language Indonesian = new Language(SystemLanguage.Indonesian, "Indonesian", "id");
        static public readonly Language Italian = new Language(SystemLanguage.Italian, "Italian", "it");
        static public readonly Language Japanese = new Language(SystemLanguage.Japanese, "Japanese", "ja");
        static public readonly Language Korean = new Language(SystemLanguage.Korean, "Korean", "ko");
        static public readonly Language Latvian = new Language(SystemLanguage.Latvian, "Latvian", "lv");
        static public readonly Language Lithuanian = new Language(SystemLanguage.Lithuanian, "Lithuanian", "lt");
        static public readonly Language Norwegian = new Language(SystemLanguage.Norwegian, "Norwegian", "nb");
        static public readonly Language Polish = new Language(SystemLanguage.Polish, "Polish", "pl");
        static public readonly Language Portuguese = new Language(SystemLanguage.Portuguese, "Portuguese", "pt");
        static public readonly Language Romanian = new Language(SystemLanguage.Romanian, "Romanian", "ro");
        static public readonly Language Russian = new Language(SystemLanguage.Russian, "Russian", "ru");
        static public readonly Language SerboCroatian = new Language(SystemLanguage.SerboCroatian, "SerboCroatian", "sr-BA");
        static public readonly Language Slovak = new Language(SystemLanguage.Slovak, "Slovak", "sk");
        static public readonly Language Slovenian = new Language(SystemLanguage.Slovenian, "Slovenian", "sl");
        static public readonly Language Spanish = new Language(SystemLanguage.Spanish, "Spanish", "es");
        static public readonly Language Swedish = new Language(SystemLanguage.Swedish, "Swedish", "sv");
        static public readonly Language Thai = new Language(SystemLanguage.Thai, "Thai", "th");
        static public readonly Language Turkish = new Language(SystemLanguage.Turkish, "Turkish", "tr");
        static public readonly Language Ukrainian = new Language(SystemLanguage.Ukrainian, "Ukrainian", "uk");
        static public readonly Language Vietnamese = new Language(SystemLanguage.Vietnamese, "Vietnamese", "vi");
        static public readonly Language ChineseSimplified = new Language(SystemLanguage.ChineseSimplified, "ChineseSimplified", "zh-CN");
        static public readonly Language ChineseTraditional = new Language(SystemLanguage.ChineseTraditional, "ChineseTraditional", "zh-TW");
        static public readonly Language Unknown = new Language(SystemLanguage.Unknown, "Unknown", "Unknown");

        public static Language systemLanguage {
            get { return GetLanguageBySL(Application.systemLanguage); }
        }

        public static bool IsLanguageCode(string code) { return GetLanguageByCode(code) != Unknown; }
        public static bool IsLanguageName(string name) { return GetLanguageByName(name) != Unknown; }

        public static Language[] GetAllLanguages() {
            return new Language[] {
                Afrikaans ,
                Arabic ,
                Basque ,
                Belarusian ,
                Bulgarian ,
                Catalan ,
                Chinese ,
                Czech ,
                Danish ,
                Dutch ,
                English ,
                Estonian ,
                Faroese ,
                Finnish ,
                French ,
                German ,
                Greek ,
                Hebrew ,
                Hungarian ,
                Icelandic ,
                Indonesian ,
                Italian ,
                Japanese ,
                Korean ,
                Latvian ,
                Lithuanian ,
                Norwegian ,
                Polish ,
                Portuguese ,
                Romanian ,
                Russian ,
                SerboCroatian ,
                Slovak ,
                Slovenian ,
                Spanish ,
                Swedish ,
                Thai ,
                Turkish ,
                Ukrainian ,
                Vietnamese ,
                ChineseSimplified ,
                ChineseTraditional ,
            };
        }

        static public Language GetLanguageByCode(string code) {
            Language language;
            GetLanguageByCode(code, out language);
            return language;
        }

        static public bool GetLanguageByCode(string code, out Language language) {
            switch (code) {
                case "af":
                    language = Afrikaans;
                    return true;
                case "ar":
                    language = Arabic;
                    return true;
                case "eu":
                    language = Basque;
                    return true;
                case "be":
                    language = Belarusian;
                    return true;
                case "bg":
                    language = Bulgarian;
                    return true;
                case "ca":
                    language = Catalan;
                    return true;
                case "zh":
                    language = Chinese;
                    return true;
                case "cs":
                    language = Czech;
                    return true;
                case "da":
                    language = Danish;
                    return true;
                case "nl":
                    language = Dutch;
                    return true;
                case "en":
                    language = English;
                    return true;
                case "et":
                    language = Estonian;
                    return true;
                case "fo":
                    language = Faroese;
                    return true;
                case "fi":
                    language = Finnish;
                    return true;
                case "fr":
                    language = French;
                    return true;
                case "de":
                    language = German;
                    return true;
                case "el":
                    language = Greek;
                    return true;
                case "he":
                    language = Hebrew;
                    return true;
                case "hu":
                    language = Hungarian;
                    return true;
                case "is":
                    language = Icelandic;
                    return true;
                case "id":
                    language = Indonesian;
                    return true;
                case "it":
                    language = Italian;
                    return true;
                case "ja":
                    language = Japanese;
                    return true;
                case "ko":
                    language = Korean;
                    return true;
                case "lv":
                    language = Latvian;
                    return true;
                case "lt":
                    language = Lithuanian;
                    return true;
                case "nb":
                    language = Norwegian;
                    return true;
                case "pl":
                    language = Polish;
                    return true;
                case "pt":
                    language = Portuguese;
                    return true;
                case "ro":
                    language = Romanian;
                    return true;
                case "ru":
                    language = Russian;
                    return true;
                case "sr-BA":
                    language = SerboCroatian;
                    return true;
                case "sk":
                    language = Slovak;
                    return true;
                case "sl":
                    language = Slovenian;
                    return true;
                case "es":
                    language = Spanish;
                    return true;
                case "sv":
                    language = Swedish;
                    return true;
                case "th":
                    language = Thai;
                    return true;
                case "tr":
                    language = Turkish;
                    return true;
                case "uk":
                    language = Ukrainian;
                    return true;
                case "vi":
                    language = Vietnamese;
                    return true;
                case "zh-CN":
                    language = ChineseSimplified;
                    return true;
                case "zh-TW":
                    language = ChineseTraditional;
                    return true;
                default:
                    language = Unknown;
                    return false;
            }
        }

        public static Language GetLanguageByName(string name) {
            Language language;
            GetLanguageByName(name, out language);
            return language;
        }

        public static bool GetLanguageByName(string name, out Language language) {
            switch (name) {
                case "Afrikaans":
                    language = Afrikaans;
                    return true;
                case "Arabic":
                    language = Arabic;
                    return true;
                case "Basque":
                    language = Basque;
                    return true;
                case "Belarusian":
                    language = Belarusian;
                    return true;
                case "Bulgarian":
                    language = Bulgarian;
                    return true;
                case "Catalan":
                    language = Catalan;
                    return true;
                case "Chinese":
                    language = Chinese;
                    return true;
                case "Czech":
                    language = Czech;
                    return true;
                case "Danish":
                    language = Danish;
                    return true;
                case "Dutch":
                    language = Dutch;
                    return true;
                case "English":
                    language = English;
                    return true;
                case "Estonian":
                    language = Estonian;
                    return true;
                case "Faroese":
                    language = Faroese;
                    return true;
                case "Finnish":
                    language = Finnish;
                    return true;
                case "French":
                    language = French;
                    return true;
                case "German":
                    language = German;
                    return true;
                case "Greek":
                    language = Greek;
                    return true;
                case "Hebrew":
                    language = Hebrew;
                    return true;
                case "Hungarian":
                    language = Hungarian;
                    return true;
                case "Icelandic":
                    language = Icelandic;
                    return true;
                case "Indonesian":
                    language = Indonesian;
                    return true;
                case "Italian":
                    language = Italian;
                    return true;
                case "Japanese":
                    language = Japanese;
                    return true;
                case "Korean":
                    language = Korean;
                    return true;
                case "Latvian":
                    language = Latvian;
                    return true;
                case "Lithuanian":
                    language = Lithuanian;
                    return true;
                case "Norwegian":
                    language = Norwegian;
                    return true;
                case "Polish":
                    language = Polish;
                    return true;
                case "Portuguese":
                    language = Portuguese;
                    return true;
                case "Romanian":
                    language = Romanian;
                    return true;
                case "Russian":
                    language = Russian;
                    return true;
                case "SerboCroatian":
                    language = SerboCroatian;
                    return true;
                case "Slovak":
                    language = Slovak;
                    return true;
                case "Slovenian":
                    language = Slovenian;
                    return true;
                case "Spanish":
                    language = Spanish;
                    return true;
                case "Swedish":
                    language = Swedish;
                    return true;
                case "Thai":
                    language = Thai;
                    return true;
                case "Turkish":
                    language = Turkish;
                    return true;
                case "Ukrainian":
                    language = Ukrainian;
                    return true;
                case "Vietnamese":
                    language = Vietnamese;
                    return true;
                case "ChineseSimplified":
                    language = ChineseSimplified;
                    return true;
                case "ChineseTraditional":
                    language = ChineseTraditional;
                    return true;
                default:
                    language = Unknown;
                    return false;
            }
        }

        public static Language GetLanguageBySL(SystemLanguage systemLanguage) {
            Language language;
            GetLanguageBySL(systemLanguage, out language);
            return language;
        }
        public static bool GetLanguageBySL(SystemLanguage systemLanguage, out Language language) {
            switch (systemLanguage) {
                case SystemLanguage.Afrikaans:
                    language = Afrikaans;
                    return true;
                case SystemLanguage.Arabic:
                    language = Arabic;
                    return true;
                case SystemLanguage.Basque:
                    language = Basque;
                    return true;
                case SystemLanguage.Belarusian:
                    language = Belarusian;
                    return true;
                case SystemLanguage.Bulgarian:
                    language = Bulgarian;
                    return true;
                case SystemLanguage.Catalan:
                    language = Catalan;
                    return true;
                case SystemLanguage.Chinese:
                    language = Chinese;
                    return true;
                case SystemLanguage.Czech:
                    language = Czech;
                    return true;
                case SystemLanguage.Danish:
                    language = Danish;
                    return true;
                case SystemLanguage.Dutch:
                    language = Dutch;
                    return true;
                case SystemLanguage.English:
                    language = English;
                    return true;
                case SystemLanguage.Estonian:
                    language = Estonian;
                    return true;
                case SystemLanguage.Faroese:
                    language = Faroese;
                    return true;
                case SystemLanguage.Finnish:
                    language = Finnish;
                    return true;
                case SystemLanguage.French:
                    language = French;
                    return true;
                case SystemLanguage.German:
                    language = German;
                    return true;
                case SystemLanguage.Greek:
                    language = Greek;
                    return true;
                case SystemLanguage.Hebrew:
                    language = Hebrew;
                    return true;
                case SystemLanguage.Hungarian:
                    language = Hungarian;
                    return true;
                case SystemLanguage.Icelandic:
                    language = Icelandic;
                    return true;
                case SystemLanguage.Indonesian:
                    language = Indonesian;
                    return true;
                case SystemLanguage.Italian:
                    language = Italian;
                    return true;
                case SystemLanguage.Japanese:
                    language = Japanese;
                    return true;
                case SystemLanguage.Korean:
                    language = Korean;
                    return true;
                case SystemLanguage.Latvian:
                    language = Latvian;
                    return true;
                case SystemLanguage.Lithuanian:
                    language = Lithuanian;
                    return true;
                case SystemLanguage.Norwegian:
                    language = Norwegian;
                    return true;
                case SystemLanguage.Polish:
                    language = Polish;
                    return true;
                case SystemLanguage.Portuguese:
                    language = Portuguese;
                    return true;
                case SystemLanguage.Romanian:
                    language = Romanian;
                    return true;
                case SystemLanguage.Russian:
                    language = Russian;
                    return true;
                case SystemLanguage.SerboCroatian:
                    language = SerboCroatian;
                    return true;
                case SystemLanguage.Slovak:
                    language = Slovak;
                    return true;
                case SystemLanguage.Slovenian:
                    language = Slovenian;
                    return true;
                case SystemLanguage.Spanish:
                    language = Spanish;
                    return true;
                case SystemLanguage.Swedish:
                    language = Swedish;
                    return true;
                case SystemLanguage.Thai:
                    language = Thai;
                    return true;
                case SystemLanguage.Turkish:
                    language = Turkish;
                    return true;
                case SystemLanguage.Ukrainian:
                    language = Ukrainian;
                    return true;
                case SystemLanguage.Vietnamese:
                    language = Vietnamese;
                    return true;
                case SystemLanguage.ChineseSimplified:
                    language = ChineseSimplified;
                    return true;
                case SystemLanguage.ChineseTraditional:
                    language = ChineseTraditional;
                    return true;
                default:
                    language = Unknown;
                    return false;
            }
        }
    }
}