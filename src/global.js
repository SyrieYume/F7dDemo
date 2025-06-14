import { ref } from "vue";
import yaml from "js-yaml";

export const currentView = ref("StartView")

export const Global = {
    viewArgs : null,
    config: {
        playerName: "指挥使"
    },

    gotoView: (name, args) => {
        if(args)
            Global.viewArgs = args
        else
            Global.viewArgs = null
        currentView.value = name
    },

    loadConfig: () => {
        const configYamlStr = localStorage.getItem("config")
        if(configYamlStr) {
            const newConfig = yaml.load(configYamlStr)
            for(let key in Global.config) {
                const value = newConfig[key]
                if(value != undefined && typeof(value) == typeof(Global.config[key]))
                    Global.config[key] = value
            }
        }
    },

    saveConfig: () => {
        const configYamlStr = yaml.dump(Global.config)
        localStorage.setItem("config", configYamlStr)
    },

    setConfig: newConfig => {
        if(newConfig) {
            for(let key in Global.config) {
                const value = newConfig[key]
                if(value != undefined && typeof(value) == typeof(Global.config[key]))
                    Global.config[key] = value
            }
            Global.saveConfig()
        }
    },
}


Global.loadConfig()