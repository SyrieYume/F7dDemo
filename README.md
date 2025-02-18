## 如何运行程序
1. 从 [Releases](https://github.com/SyrieYume/F7dDemo/releases) 下载 **v0.2.0** 版的 `F7dDemo.zip`

2. 将 `F7dDemo.zip` 解压到任意位置

3. 运行其中的 `永远的7日之都(Demo).exe`

4. 按照控制台提示，用浏览器打开 `http://localhost:3051` (默认是使用 3051 端口，如果这个端口被占用，就是其它端口)

5. 如果遇到 **Bug**，欢迎在 [Issues](https://github.com/SyrieYume/F7dDemo/issues) 中提出

## 使用说明
1. 用 文本编辑器 编辑程序目录下的 `config.yaml` 可以修改显示的玩家名字

2. 剧情中按 `K` 键切换 **自动播放剧情**

## 如何手动编译本项目

### Web前端部分
1. 下载和安装 [**Node.js**](https://nodejs.org/)
   
2. 下载项目的源代码，解压到任意目录

3. 从 [Releases](https://github.com/SyrieYume/F7dDemo/releases) 下载 **v0.2.0** 版的 `F7dDemo.zip`

4. 将其中的 `/res/audio` 和 `/res/bgm` 分别复制到项目文件夹下的 `/public/res/audio` 和 `/public/res/bgm`

5. 在项目目录下执行以下命令：
```powershell
npm install
npm run build
```
6. 生成的文件在项目目录的 `dist` 路径下
