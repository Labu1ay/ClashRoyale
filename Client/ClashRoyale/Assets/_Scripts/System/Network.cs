using System;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Threading.Tasks;

namespace Plugins.Network.Scripts {
    public class Network {

        public async void Post(string uri, Dictionary<string, string> data, Action<string> success,
            Action<string> error = null) =>
            await PostAsync(uri, data, success, error);

        private async Task PostAsync(string uri, Dictionary<string, string> data, Action<string> success,
            Action<string> error = null) {
            using (UnityWebRequest www = UnityWebRequest.Post(uri, data)) {
                www.SendWebRequest();
                while (!www.isDone) {
                    await Task.Yield();
                }

                if (www.result != UnityWebRequest.Result.Success) error?.Invoke(www.error);
                else success?.Invoke(www.downloadHandler.text);
            }
        }
    }
}