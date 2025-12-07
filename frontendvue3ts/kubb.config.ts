import {defineConfig} from '@kubb/core'
import {pluginOas} from '@kubb/plugin-oas'
import {pluginVueQuery} from '@kubb/plugin-vue-query'
import {pluginTs} from '@kubb/plugin-ts'
import {pluginClient} from "@kubb/plugin-client";

export default defineConfig
({
    input: {
        path: 'http://localhost:8081/swagger/v1/swagger.json'
    },
    output: {
        path: './src/gen',
        clean: true
    },
    plugins: [
        pluginOas
        (),
        pluginTs
        (),
        pluginClient({
            output: {
                path: './clients/axios',
                barrelType: 'named',
                banner: '/* eslint-disable no-alert, no-console */',
                footer: ''
            },
            group: {
                type: 'tag',
                name: ({ group }) => `${group}Service`,
            },
            transformers: {
                name: (name, type) => {
                    return `${name}Client`
                },
            },
            operations: true,
            parser: 'client',
            exclude: [
                {
                    type: 'tag',
                    pattern: 'store',
                },
            ],
            pathParamsType: "object",
            dataReturnType: 'full',
            client: 'axios'
        }),
    ],
})