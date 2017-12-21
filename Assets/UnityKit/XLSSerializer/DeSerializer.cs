using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

namespace UnityKit {

    public interface ITextAssetReader {
        TextAsset Read(string filename);
    }


    public class DeSerializer {

        ITextAssetReader _reader;
        public DeSerializer(ITextAssetReader reader) {
            _reader = reader;
        }

        public T DeSerialize<T>(Stream stream) {
            var binFormatter = new BinaryFormatter();
            var ret = (T)binFormatter.Deserialize(stream);
            return ret;
        }

        public T DeSerialize<T>(string filename) {
            T ret;
            var textasset = _reader.Read(filename);
            using (var ms = new MemoryStream(textasset.bytes)) {
                ret = DeSerialize<T>(ms);
            }
            return ret;
        }
    }
}