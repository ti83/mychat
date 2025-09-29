<script setup lang="ts">
import ConversationList from './ConversationList.vue'
import ActiveConversation from './ActiveConversation.vue'
</script>

<template>
  <div class="chat-container">
    <div class="conversation-list">
      <ConversationList />
    </div>
    <div class="active-conversation">
      <ActiveConversation />
    </div>
  </div>
</template>

<style scoped>
.chat-container {
  display: flex;
  width: 100%;
  height: 100vh;
  overflow: hidden;
}

.conversation-list {
  width: 300px;
  border-right: 1px solid #e0e0e0;
  overflow-y: auto;
}

.active-conversation {
  flex: 1;
  overflow-y: auto;
  padding: 1rem;
}
</style>
