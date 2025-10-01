<script lang="ts" setup>
import { ref, onMounted as vueOnMounted } from 'vue'
import type { Conversation } from '@/types/Conversation'
import type { Message } from '@/types/Message'
import ConversationApiService from '@/services/ConversationApiService'
import MessageItem from './MessageItem.vue'
import { ConversationStore } from '@/store/ConversationStore'

const newMessage = ref('')
const currentConversation = ref<Conversation | null>(null)
const isRequestProcessing = ref(false)
const apiService = new ConversationApiService()
const store = ConversationStore()

async function sendMessage(): Promise<void> {
  if (!newMessage.value.trim()) {
    return
  }
  isRequestProcessing.value = true
  await createNewConversation()
  const now = new Date()
  const newMsg: Message = {
    id: 0,
    timestamp: new Date(now.toUTCString()),
    text: newMessage.value,
    source: 'user',
  }
  currentConversation.value?.messages.push(newMsg)
  newMessage.value = ''
  const returnedMessage: Message = await apiService.SendMessage(
    currentConversation.value!.id,
    newMsg,
  )

  if (!returnedMessage) {
    console.error('No response from server')
    isRequestProcessing.value = false
    return
  }

  currentConversation.value?.messages.push(returnedMessage)

  UpdateTitle()

  isRequestProcessing.value = false
}

async function UpdateTitle(): Promise<void> {
  if (!currentConversation.value) {
    return
  }
  const header = await apiService.GetConversationHeader(currentConversation.value.id)
  if (header) {
    currentConversation.value.title = header.title
    store.updateTitle(store, header.id, header.title)
  }
}

async function createNewConversation(): Promise<void> {
  if (currentConversation.value?.id != 0) {
    return
  }

  const newConversation = await apiService.CreateNewConversation()
  if (newConversation) {
    currentConversation.value.id = newConversation.id
    store.addConversation(store, newConversation)
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
    <div class="conversation-header">
      <h2>{{ currentConversation?.title || 'New Conversation' }}</h2>
    </div>

    <div class="message-container">
      <div v-if="currentConversation">
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

    <div id="messageInput">
      <textarea
        type="text"
        :disabled="isRequestProcessing"
        name="newMessage"
        id="newMessage"
        placeholder="Poke and prod, I'm ready..."
        v-model="newMessage"
      >
      </textarea>
      <button @click="sendMessage" :disabled="isRequestProcessing">Send</button>
    </div>
  </div>
</template>

<style scoped>
.active-conversation {
  max-width: 1024px;
  min-width: 800px;
  margin: 0 auto;
  font-family: Arial, sans-serif;
  display: flex;
  max-height: 80vh;
  flex-direction: column;
}

.conversation-header {
  position: sticky;
  top: 0;
  background-color: white;
  padding: 0 0;
  border-bottom: 1px solid #eee;
  z-index: 1;
}

.message-container {
  flex: 1;
  overflow-y: auto;
  padding: 0 0;
}

#messageInput {
  border-top: 1px solid #eee;
  padding: 10px 0;
  background-color: white;
  display: flex;
  gap: 8px;
}

#newMessage {
  flex: 1;
  padding: 8px;
  box-sizing: border-box;
  max-height: 100px;
}

button {
  align-self: flex-start;
  padding: 8px 16px;
}
</style>
