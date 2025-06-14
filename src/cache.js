export const ImageCache = {
    cache: new Map(),

    add: async imgUrl => {
        if (ImageCache.cache.has(imgUrl)) 
            return  
        const res = await fetch(imgUrl)
        if (!res.ok) 
            throw new Error('Failed to fetch ' + imgUrl)
        const blob = await res.blob()
        const objectURL = URL.createObjectURL(blob)
        ImageCache.cache.set(imgUrl, objectURL)
    },
  
    remove: imgUrl => {
        if (ImageCache.cache.has(imgUrl)) {
            URL.revokeObjectURL(ImageCache.cache.get(imgUrl))
            ImageCache.cache.delete(imgUrl)
        }
    },
  
    get: imgUrl => ImageCache.cache.get(imgUrl) || null,
  
    preload: async urls => {
        for(const url of urls)
            await ImageCache.add(url)
    },

    clearAll: () => {
        for(const [imgUrl, objectURL] of ImageCache.cache) {
            URL.revokeObjectURL(objectURL)
            ImageCache.cache.delete(imgUrl)
        }
    }
}