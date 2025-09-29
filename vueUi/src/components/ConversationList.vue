<script lang="ts">
export default {
  name: 'ConversationList',
  data() {
    return {
      conversations: [
        { id: 1, title: 'Project Discussion' },
        { id: 2, title: 'Family Group' },
        { id: 3, title: 'Book Club' },
        { id: 4, title: 'Tech Talk' },
        { id: 5, title: 'Weekend Plans' },
      ],
    }
  },
  methods: {
    deleteConversation(index: number) {
      this.conversations.splice(index, 1)
    },
  },
}
</script>

<template>
  <div class="conversation-list">
    <h2>Conversations</h2>
    <div
      v-for="(conversation, index) in conversations"
      :key="conversation.id"
      class="conversation-item"
    >
      <span>{{ conversation.title }}</span>
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
