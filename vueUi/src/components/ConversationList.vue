<script lang="ts" setup>
import { ConversationStore } from '@/store/ConversationStore'
import type { ConversationHeader } from '@/types/ConversationHeader'
import ConversationApiService from '@/services/ConversationApiService'

const store = ConversationStore()
const apiService = new ConversationApiService()
function deleteConversation(index: number) {
  const conversation: ConversationHeader = store.conversations[index]
  if (conversation.id === 0) {
    store.conversations.splice(index, 1)
    return
  }
  apiService.DeleteConversation(conversation.id).then((response) => {
    if (response) {
      store.deleteConversation(store, conversation.id)
    } else {
      console.error('Failed to delete conversation')
    }
  })
}
</script>

<template>
  <div class="conversation-list">
    <h2>Conversations</h2>
    <div
      v-for="(conversation, index) in store.conversations"
      :key="conversation.id"
      class="conversation-item"
    >
      <span>{{ conversation.title ? conversation.title : 'New Conversation' }}</span>
      <button @click="deleteConversation(index)">Delete</button>
    </div>
  </div>
</template>

<style scoped>
.conversation-list {
  max-width: 400px;
  margin: 0 auto;
  font-family: Arial, sans-serif;
}
.conversation-item {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 8px 8px 0px 8px;
  border-bottom: 1px solid #eee;
}
button {
  background: #e74c3c;
  color: white;
  border: none;
  padding: 4px 10px;
  border-radius: 4px;
  cursor: pointer;
}
button:hover {
  background: #c0392b;
}
</style>
