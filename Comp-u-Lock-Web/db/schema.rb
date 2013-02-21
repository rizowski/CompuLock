# encoding: UTF-8
# This file is auto-generated from the current state of the database. Instead
# of editing this file, please use the migrations feature of Active Record to
# incrementally modify your database, and then regenerate this schema definition.
#
# Note that this schema.rb definition is the authoritative source for your
# database schema. If you need to create the application database on another
# system, you should be using db:schema:load, not running all the migrations
# from scratch. The latter is a flawed and unsustainable approach (the more migrations
# you'll amass, the slower it'll run and the greater likelihood for issues).
#
# It's strongly recommended to check this file into your version control system.

ActiveRecord::Schema.define(:version => 20130220064805) do

  create_table "account_histories", :force => true do |t|
    t.integer  "account_id"
    t.string   "domain",      :default => "", :null => false
    t.string   "url",         :default => "", :null => false
    t.string   "title",       :default => "", :null => false
    t.integer  "visit_count", :default => 0,  :null => false
    t.datetime "created_at",                  :null => false
    t.datetime "updated_at",                  :null => false
  end

  create_table "account_processes", :force => true do |t|
    t.integer  "account_id"
    t.string   "name",       :default => "", :null => false
    t.datetime "created_at",                 :null => false
    t.datetime "updated_at",                 :null => false
  end

  create_table "account_programs", :force => true do |t|
    t.integer  "account_id"
    t.string   "name",       :default => "", :null => false
    t.integer  "open_count", :default => 0,  :null => false
    t.datetime "created_at",                 :null => false
    t.datetime "updated_at",                 :null => false
  end

  create_table "accounts", :force => true do |t|
    t.integer  "computer_id"
    t.string   "domain",        :default => "",    :null => false
    t.string   "username",                         :null => false
    t.boolean  "tracking",      :default => false, :null => false
    t.time     "allotted_time"
    t.time     "used_time"
    t.datetime "created_at",                       :null => false
    t.datetime "updated_at",                       :null => false
  end

  create_table "computers", :force => true do |t|
    t.integer  "user_id"
    t.string   "name",       :default => "", :null => false
    t.string   "ip_address", :default => "", :null => false
    t.string   "enviroment", :default => "", :null => false
    t.datetime "created_at",                 :null => false
    t.datetime "updated_at",                 :null => false
  end

  create_table "days", :force => true do |t|
    t.integer  "restriction_id"
    t.datetime "created_at",     :null => false
    t.datetime "updated_at",     :null => false
  end

  create_table "hours", :force => true do |t|
    t.integer  "day_id"
    t.integer  "start_time"
    t.integer  "end_time"
    t.datetime "created_at", :null => false
    t.datetime "updated_at", :null => false
  end

  create_table "restrictions", :force => true do |t|
    t.integer  "account_id"
    t.datetime "created_at", :null => false
    t.datetime "updated_at", :null => false
  end

  create_table "users", :force => true do |t|
    t.string   "username"
    t.string   "email",                  :default => "",    :null => false
    t.string   "encrypted_password",     :default => "",    :null => false
    t.boolean  "admin",                  :default => false
    t.string   "reset_password_token"
    t.datetime "reset_password_sent_at"
    t.datetime "remember_created_at"
    t.integer  "sign_in_count",          :default => 0
    t.datetime "current_sign_in_at"
    t.datetime "last_sign_in_at"
    t.string   "current_sign_in_ip"
    t.string   "last_sign_in_ip"
    t.datetime "created_at",                                :null => false
    t.datetime "updated_at",                                :null => false
    t.string   "authentication_token"
  end

  add_index "users", ["authentication_token"], :name => "index_users_on_authentication_token", :unique => true
  add_index "users", ["email"], :name => "index_users_on_email", :unique => true
  add_index "users", ["reset_password_token"], :name => "index_users_on_reset_password_token", :unique => true

end
