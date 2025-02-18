// 防抖函数
export function debounce(fn, delay) {
    let timer = null
    return function (...args) {
        const context = this
        clearTimeout(timer)
        timer = setTimeout(() => {
            fn.apply(context, args)
            timer = null
        }, delay)
    }
}


// 简单的动画关键帧生成以及动画融合
export const Ani = {
    fade: (start, end) => [ { opacity: start }, { opacity: end } ],
    scale: (start, end) => [ { scale: start }, { scale: end } ],

    blend: (keyframes1, keyframes2) => {
        return keyframes1.map((keyframe, i) =>
            Object.assign({}, keyframe, keyframes2[i] )
        );
    }
}


// 每隔interval ms执行func，当condition()不满足时结束
export function setIntervalAsync(func, interval, condition) {
    return new Promise((resolve) => {
        let timer = null

        const callback = () => {
            if(condition())
                func()
            else {
                clearInterval(timer)
                resolve()
            }
        }
        timer = setInterval(callback, interval)
    })
}


// sleep函数
export function sleep(milliseconds) {
    return new Promise((resolve) => setTimeout(resolve , milliseconds) )
}


// 等待至 codition() 为 true
export function waitUntil(condition) {
    return new Promise((resolve) => {
        const check = () => {
            if(condition()) resolve()
            else requestAnimationFrame(check)
        }
        check()
    })
}


// 设置audio标签的音频数据 (用于提前加载整个音频的数据，否则无法使用 currentTime 调整播放位置)
export async function setAudio(audioElement, audioSrc) {
    if(audioSrc.length == 0) {
        audioElement.src = ""
        return
    }

    if(audioElement.src.trim() != "")
        URL.revokeObjectURL(audioElement.src)

    const res = await fetch(audioSrc)
    if(!res.ok)
        throw new Error("Faild to load audio " + audioSrc)
    const blob = await res.blob()
    const blobUrl = URL.createObjectURL(blob)
    audioElement.src = blobUrl
    await new Promise((resolve) => {
        audioElement.addEventListener("canplaythrough", resolve, { once: true })
    })
} 


// 切换音频（有过渡效果）
export async function audioSwitch(audioElement, newAudioSrc) {
    const originalVolume = audioElement.volume
    
    // 淡出过渡
    if(audioElement.src.length > 0){
        const duration = 800
        const startTime = Date.now()

        await new Promise((resolve) => {
            const transition = () => {
                const elapsed = Date.now() - startTime
                const progress = Math.min(elapsed / duration, 1)

                audioElement.volume = originalVolume * (1 - progress)

                if(progress < 1)
                    requestAnimationFrame(transition)
                else 
                    resolve()
            }
            
            transition()
        })
    }

    await setAudio(audioElement, newAudioSrc)
    audioElement.volume = originalVolume
    await audioElement.play()
}



// 将格式为 HH:MM:SS.milliseconds 的时间字符串转换为毫秒值
function timeToMilliseconds(timeString) {
    timeString = timeString.trim()

    const regex = /^.*(\d{2}):(\d{2})\.(\d{3})$/
    if (!regex.test(timeString))
        throw new Error("Invalid time format. Expected HH:MM:SS:FF.milliseconds")
    
    const parts = timeString.match(regex)
    const minutes = parseInt(parts[1], 10)
    const seconds = parseInt(parts[2], 10)
    const milliseconds = parseInt(parts[3], 10)
 
    return minutes * 60 * 1000 + seconds * 1000 + milliseconds
}


// 播放音频
export function playAudio(audioElement, startTime, endTime, condition = () => true) {
    const startSecond = timeToMilliseconds(startTime) / 1000
    const endSecond = timeToMilliseconds(endTime) / 1000

    return new Promise((resolve, reject) => {
        // 清理函数
        const cleanup = () => {
            audioElement.removeEventListener('ended', onEnded)
            audioElement.pause()
        };

        // 精准时间检查
        const checkTime = () => {
            if (audioElement.currentTime >= endSecond || !condition()) {
                cleanup()
                resolve()
            } 
            else requestAnimationFrame(checkTime)
        };

        // 处理自然结束
        const onEnded = () => {
            cleanup()
            resolve()
        };

        // 设置起始时间并播放
        audioElement.currentTime = startSecond
        audioElement.play()
        .then(() => {
            audioElement.addEventListener('ended', onEnded)
            checkTime()
        })
        .catch((err) => {
            cleanup();
            reject(err);
        })
    })
}


// 加载yaml文件
export async function loadYaml(yamlSrc) {
    const yaml = await import("js-yaml")
    
    const res = await fetch(yamlSrc)
    if(res.status != 200)
        throw new Error("Failed to load yaml: " + yamlSrc)
    
    const scriptText = await res.text()
    return yaml.load(scriptText)
}