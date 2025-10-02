<script lang="ts" setup>
import { computed, ref, onMounted as vueOnMounted } from 'vue'
import type { Message } from '@/types/Message'
import ConversationApiService from '@/services/ConversationApiService'
import MessageItem from './MessageItem.vue'
import { ConversationStore } from '@/store/ConversationStore'

const defaultTitle: string = 'New Conversation'
const newMessage = ref('')
const isEditingTitle = ref(false)
const editableTitle = ref('')
//const currentConversation = computed(() => store.currentConversation)
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
  store.currentConversation?.messages.push(newMsg)
  newMessage.value = ''
  const returnedMessage: Message = await apiService.SendMessage(
    store.currentConversation!.id,
    newMsg,
  )

  if (!returnedMessage) {
    console.error('No response from server')
    isRequestProcessing.value = false
    return
  }

  store.currentConversation?.messages.push(returnedMessage)

  UpdateTitle()

  isRequestProcessing.value = false
}

async function UpdateTitle(): Promise<void> {
  if (!store.currentConversation) {
    return
  }
  const header = await apiService.GetConversationHeader(store.currentConversation.id)
  if (header) {
    store.currentConversation.title = header.title
    store.updateTitle(store, header.id, header.title)
  }
}

async function createNewConversation(): Promise<void> {
  if (store.currentConversation?.id != 0) {
    return
  }
  const newTitle =
    store.currentConversation?.title == defaultTitle ? '' : store.currentConversation?.title
  const newConversation = await apiService.CreateNewConversation(newTitle)
  if (newConversation) {
    store.currentConversation.id = newConversation.id
    store.addConversation(store, newConversation)
  }
}

function mounted() {
  // Simulate loading a conversation
  store.currentConversation = {
    id: 0,
    title: '',
    messages: [],
  }
}

function startEditingTitle() {
  editableTitle.value = store.currentConversation?.title || defaultTitle
  isEditingTitle.value = true
}

async function saveTitle() {
  if (!store.currentConversation) return

  const newTitle = editableTitle.value.trim()
  if (newTitle && store.currentConversation.id !== 0) {
    const success = await apiService.UpdateConversationTitle(store.currentConversation.id, newTitle)
    if (success) {
      store.currentConversation.title = newTitle
      store.updateTitle(store, store.currentConversation.id, newTitle)
    }
  } else if (newTitle && store.currentConversation.id === 0) {
    // If it's a new conversation, just update the title locally
    store.currentConversation.title = newTitle
  }
  isEditingTitle.value = false
}

vueOnMounted(mounted)
</script>

<template>
  <div class="active-conversation">
    <div class="conversation-header">
      <div v-if="isEditingTitle" class="title-edit">
        <input
          v-model="editableTitle"
          @blur="saveTitle"
          @keyup.enter="saveTitle"
          @keyup.esc="isEditingTitle = false"
          ref="titleInput"
          type="text"
        />
      </div>
      <h2 v-else @click="startEditingTitle">
        {{ store.currentConversation?.title || defaultTitle }}
      </h2>
    </div>

    <div class="message-container">
      <div v-if="store.currentConversation">
        <div
          v-for="(message, index) in store.currentConversation.messages"
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

.title-edit input {
  font-size: 1.5em;
  font-weight: normal;
  width: 100%;
  padding: 5px;
  border: 1px solid #ddd;
  border-radius: 4px;
}
</style>
