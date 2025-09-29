<script lang="ts" setup>
import { ref, onMounted as vueOnMounted } from 'vue'
import type { Conversation } from '@/types/Conversation'
import type { Message } from '@/types/Message'
import MessageItem from './MessageItem.vue'

const newMessage = ref('tell me a story about a cowboy')
const currentConversation = ref<Conversation | null>(null)
const isRequestProcessing = ref(false)

async function sendMessage(): Promise<void> {
  if (!newMessage.value.trim()) {
    return
  }
  isRequestProcessing.value = true
  await createNewConversation()
  console.trace('Send message:', newMessage.value)
  const now = new Date()
  const newMsg: Message = {
    timestamp: new Date(now.toUTCString()),
    text: newMessage.value,
    source: 'user',
  }

  currentConversation.value?.messages.push(newMsg)
  const url = `https://localhost:7192/conversation/ask/${currentConversation.value?.id}`
  try {
    console.trace('messages being sent to server:' + JSON.stringify(newMsg))
    const response = await fetch(url, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(newMsg),
    })
    if (!response.ok) {
      throw new Error(`HTTP error! status: ${response.status}`)
    }
    const returnedMessage: Message = await response.json()
    console.log('Received message from server:', returnedMessage)
    currentConversation.value?.messages.push(returnedMessage)
    newMessage.value = ''
  } catch (error) {
    console.error('Error sending message:', error)
  }
  isRequestProcessing.value = false
}

async function createNewConversation(): Promise<void> {
  if (currentConversation.value?.id != 0) {
    return
  }

  try {
    const response = await fetch('https://localhost:7192/conversation', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({ title: currentConversation.value?.title }),
    })
    if (!response.ok) {
      throw new Error(`HTTP error! status: ${response.status}`)
    }
    const createdConversation: Conversation = await response.json()
    currentConversation.value.id = createdConversation.id
    console.log('Created conversation with ID:', createdConversation.id)
  } catch (error) {
    console.error('Error creating conversation:', error)
  }
}

function mounted() {
  // Simulate loading a conversation
  currentConversation.value = {
    id: 0,
    title: '',
    messages: [],
  }
}

vueOnMounted(mounted)
</script>

<template>
  <div class="active-conversation">
    <h2>Active Conversation</h2>
    <div>
      <div v-if="currentConversation">
        <h3>{{ currentConversation.title || 'New Conversation' }}</h3>
        <div
          v-for="(message, index) in currentConversation.messages"
          :key="index"
          style="margin-bottom: 10px"
        >
          <MessageItem :message="message" />
        </div>
      </div>
      <div v-else>
        <p>No active conversation. Please select or start a new conversation.</p>
      </div>
    </div>
    <div>
      <textarea
        type="text"
        :disabled="isRequestProcessing"
        name="newMessage"
        id="newMessage"
        placeholder="Type a message..."
        style="width: 100%; padding: 8px; box-sizing: border-box; max-height: 100px"
        v-model="newMessage"
      >
      </textarea>
      <button
        style="margin-top: 8px; padding: 8px 16px"
        @click="sendMessage"
        :disabled="isRequestProcessing"
      >
        Send
      </button>
    </div>
  </div>
</template>

<style scoped>
.active-conversation {
  max-width: 1024px;
  min-width: 800px;
  margin: 0 auto;
  font-family: Arial, sans-serif;
  background-color: bisque;
}
</style>
