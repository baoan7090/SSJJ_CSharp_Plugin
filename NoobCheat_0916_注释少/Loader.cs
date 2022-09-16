using __.PluginModule;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace __
{
    class Loader : MonoBehaviour
    {
		//参考https://github.com/Airahc/hack_ssjj的加载及运作方式

		Dictionary<Type, BaseModule> modules = new Dictionary<Type, BaseModule>(); //模块列表


		static void load()
        {
			GameObject plugin = new GameObject();
			plugin.AddComponent<Loader>();
            DontDestroyOnLoad(plugin);
        }
		void Start()
		{
			AddPlugin<GetNeedData>();
			AddPlugin<ESP>();
			AddPlugin<AimBot>();
			AddPlugin<AntiMouseRight>();
			AddPlugin<TriggerBot>();

			foreach (KeyValuePair<Type, BaseModule> module in modules)
			{
				try
				{
					module.Value.Start();
				}
				catch (Exception) { }
			}
		}
		void OnGUI()
		{
			foreach (KeyValuePair<Type, BaseModule> module in modules)
			{
				try { module.Value.OnGUI(); }
				catch (Exception) { }
			}
		}
		void Update()
		{
			foreach (KeyValuePair<Type, BaseModule> module in modules)
			{
				try { module.Value.Update(); }
				catch (Exception) { }
			}
		}
		//增加类
		void AddPlugin<T>() where T : BaseModule, new()
		{
			if (!modules.ContainsKey(typeof(T)))
			{
				modules.Add(typeof(T), Activator.CreateInstance(typeof(T)) as T);
			}
		}
	}
}
