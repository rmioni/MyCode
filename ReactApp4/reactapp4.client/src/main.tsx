import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import Counter from './Counter.tsx'
import Weather from './Weather.tsx'

const elemRoot = document.getElementById('root')!; // HTML elem
const reactElemRoot = createRoot(elemRoot); // React root elem

reactElemRoot.render( // replace root div with below elements
    <StrictMode>
        <Counter />
        <Weather />
    </StrictMode>,
)
