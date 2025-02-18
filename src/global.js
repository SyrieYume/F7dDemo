import { ref } from "vue";
import { loadYaml } from "./utils";

export const currentView = ref("StartView")

export const Global = {
    viewArgs : null,
    playerName: "指挥使",

    gotoView: (name, args) => {
        if(args)
            Global.viewArgs = args
        else
            Global.viewArgs = null
        currentView.value = name
    },
}

loadYaml("/config.yaml").then(data => Object.assign(Global, data))