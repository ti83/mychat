<script lang="ts" setup>
import { ConversationStore } from '@/store/ConversationStore'
import ConversationApiService from '@/services/ConversationApiService'
import type { Conversation } from '@/types/Conversation'

const store = ConversationStore()
const apiService = new ConversationApiService()
function deleteConversation(id: number) {
  const conversation = store.conversations.find((c) => c.id === id)

  if (conversation === undefined) {
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

async function selectConversation(index: number) {
  try {
    const conversation: Conversation | null = await apiService.GetFullConversation(index)
    if (conversation) {
      store.setCurrentConversation(store, conversation)
    } else {
      console.error('Conversation not found')
    }
  } catch (error) {
    console.error('Error selecting conversation:', error)
  }
}
function addNewConversation() {
  store.resetCurrentConversation(store)
}
</script>

<template>
  <div class="conversation-list">
    <div class="header">
      <h2>Conversations</h2>
      <button @click="addNewConversation()" class="add">+</button>
    </div>
    <div
      v-for="conversation in store.conversations"
      :key="conversation.id"
      class="conversation-item"
    >
      <span @click="selectConversation(conversation.id)" class="item-link">{{
        conversation.title ? conversation.title : 'New Conversation'
      }}</span>
      <button
        @click="deleteConversation(conversation.id)"
        class="delete"
        :disabled="conversation.title ? false : true"
      >
        Delete
      </button>
    </div>
  </div>
</template>

<style scoped>
.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 10px;
  margin-right: 10px;
}

.header h2 {
  margin: 0;
}
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
button.delete {
  background: #e74c3c;
  color: white;
  border: none;
  padding: 4px 10px;
  border-radius: 4px;
  cursor: pointer;
}
button.delete:hover {
  background: #c0392b;
}
button.delete:disabled {
  background: #c0c0c0;
  color: black;
  border: none;
  padding: 4px 10px;
  border-radius: 4px;
  cursor: arrow;
}
button.delete:disabled:hover {
  background: #c0c0c0;
  color: black;
  border: none;
  padding: 4px 10px;
  border-radius: 4px;
  cursor: arrow;
}
button.add {
  background: #009e00;
  color: white;
  border: none;
  padding: 4px 10px;
  border-radius: 4px;
  cursor: pointer;
}
button.add:hover {
  background: #005800;
}
span.item-link {
  cursor: pointer;
}
span.item-link:hover {
  cursor: pointer;
  text-decoration: underline;
}
</style>
