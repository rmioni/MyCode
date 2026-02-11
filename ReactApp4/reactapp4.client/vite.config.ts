import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// https://vite.dev/config/
export default defineConfig({
    plugins: [react()],
    server: {
        port: 5173,
        strictPort: true, // Prevents Vite from jumping to a random port
    },
    build: {
        sourcemap: true, // Required for your TSX breakpoints to work
    }
})
