<script lang="ts" setup>
import type { Message } from '@/types/Message'
import { computed } from 'vue'
const props = defineProps<{
  message: Message
}>()

const isBot = computed(() => props.message.source !== 'user')
</script>
<template>
  <div :class="[isBot ? 'message-item-bot' : 'message-item-user']">
    <strong
      >{{ message.source === 'user' ? 'You' : message.source }} (<small style="color: gray">{{
        new Date(message.timestamp).toLocaleString()
      }}</small
      >):</strong
    >
    <p v-html="message.text.replace(/(\r\n|\n|\r)/g, '<br />')"></p>
  </div>
</template>

<style scoped>
.message-item-bot {
  background-color: #f9f9f9;
  border: 1px solid #ddd;
  border-radius: 5px;
  padding: 10px;
  text-align: left;
}
.message-item-user {
  background-color: #c3e7ff;
  border: 1px solid #ddd;
  border-radius: 5px;
  padding: 10px;
  text-align: right;
}
</style>
