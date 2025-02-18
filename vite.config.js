import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'

// https://vite.dev/config/
export default defineConfig({
  plugins: [vue()],
  build: {
    minify: false,
    terserOptions: {
      mangle: false,  // 禁用变量名混淆
      compress: false, // 禁用代码压缩
      format: {
        beautify: true, // 美化代码
      },
    }
  }
})
