class CreateAccounts < ActiveRecord::Migration
  def change
    create_table :accounts do |t|
      t.references :computer
      
      t.string :domain
      t.string :user_name
      t.boolean :tracking
      t.time :allotted_time
      t.time :used_time

      t.timestamps
    end
  end
end
